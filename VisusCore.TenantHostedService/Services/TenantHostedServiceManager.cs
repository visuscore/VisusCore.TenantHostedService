using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Builders;
using OrchardCore.Environment.Shell.Models;
using OrchardCore.Environment.Shell.Scope;
using OrchardCore.Locking.Distributed;
using OrchardCore.Modules;
using OrchardCore.Settings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisusCore.TenantHostedService.Abstractions.Models;
using VisusCore.TenantHostedService.Abstractions.Services;
using VisusCore.TenantHostedService.Constants;
using VisusCore.TenantHostedService.Core.Models;
using VisusCore.TenantHostedService.Extensions;

namespace VisusCore.TenantHostedService.Services;

public sealed class TenantHostedServiceManager : BackgroundService, ITenantHostedServiceManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IShellHost _shellHost;
    private readonly ILogger<TenantHostedServiceManager> _logger;
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<ITenantHostedService, TenantHostedServiceStatus>> _startedServices = new();

    public TenantHostedServiceManager(
        IHttpContextAccessor httpContextAccessor,
        IShellHost shellHost,
        ILogger<TenantHostedServiceManager> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _shellHost = shellHost;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _shellHost.InitializeAsync();

        while (!GetShellContexts().Any())
        {
            if (await stoppingToken.WaitAsync(TimeSpan.FromSeconds(1)))
            {
                return;
            }
        }

        foreach (var shellContext in GetShellContexts())
        {
            await StartHostedServicesInShellContextAsync(shellContext, stoppingToken);
        }

        await stoppingToken.WaitAsync(Timeout.InfiniteTimeSpan);

        foreach (var shellContext in GetShellContexts())
        {
            await StopHostedServicesInShellContextAsync(shellContext);
        }
    }

    private Task StartHostedServicesInShellContextAsync(ShellContext shellContext, CancellationToken stoppingToken = default) =>
        UsingLockedInScopeAsync(shellContext, async scope =>
        {
            var shellFeaturesManager = scope.ServiceProvider.GetRequiredService<IShellFeaturesManager>();
            if (!(await shellFeaturesManager.GetEnabledFeaturesAsync()).Any(feature => feature.Id == FeatureIds.Loader))
            {
                return;
            }

            await SetHttpAccessorBaseUrlInScopeAsync(scope);

            var hostedServices = scope.ServiceProvider.GetServices<ITenantHostedService>();
            var shellStartedServices = _startedServices.GetOrAdd(shellContext.Settings.Name, _ => new());

            foreach (var hostedService in hostedServices)
            {
                var serviceStatus = shellStartedServices.GetOrAdd(
                    hostedService,
                    hostedService => new() { ImplementationType = hostedService.GetType() });
                if (serviceStatus.IsStarted || serviceStatus.HasError)
                {
                    continue;
                }

                try
                {
                    await hostedService.StartAsync(stoppingToken);
                    SetSuccessStatus(serviceStatus, isStarted: true);
                }
                catch (Exception exception) when (!exception.IsFatal())
                {
                    SetErrorStatus(serviceStatus, exception);
                    _logger.LogError(
                        exception,
                        "Error while starting the hosted service in the tenant '{TenantName}'.",
                        shellContext.Settings.Name);
                }
            }
        });

    private Task StopHostedServicesInShellContextAsync(ShellContext shellContext) =>
        UsingLockedInScopeAsync(shellContext, async scope =>
        {
            var shellStartedServices = _startedServices.GetOrAdd(shellContext.Settings.Name, _ => new());
            foreach (var hostedService in shellStartedServices.Keys)
            {
                var serviceStatus = shellStartedServices[hostedService];
                if (!serviceStatus.IsStarted || serviceStatus.HasError)
                {
                    continue;
                }

                try
                {
                    await hostedService.StopAsync(CancellationToken.None);
                    SetSuccessStatus(serviceStatus, isStarted: false);
                }
                catch (Exception exception) when (!exception.IsFatal())
                {
                    SetErrorStatus(serviceStatus, exception);
                    _logger.LogError(
                        exception,
                        "Error while stopping the hosted service in the tenant '{TenantName}'.",
                        shellContext.Settings.Name);
                }
            }
        });

    private IEnumerable<ShellContext> GetShellContexts() =>
        _shellHost.ListShellContexts()
            .Where(context => context.Settings.State == TenantState.Running)
            .ToList();

    private async Task UsingLockedInScopeAsync(ShellContext shellContext, Func<ShellScope, Task> inScopeAsync)
    {
        var shellScope = await _shellHost.GetScopeAsync(shellContext.Settings);

        var distributedLock = shellScope.ShellContext.ServiceProvider.GetRequiredService<IDistributedLock>();
        (var locker, var locked) = await distributedLock.TryAcquireLockAsync(
            $"{shellContext.Settings.Name}-{nameof(TenantHostedServiceManager)}",
            TimeSpan.FromSeconds(1));
        if (!locked)
        {
            _logger.LogInformation("Timeout to acquire a lock on tenant '{TenantName}'.", shellContext.Settings.Name);
            return;
        }

        await using var acquiredLock = locker;
        await shellScope.UsingAsync(inScopeAsync);
    }

    private async Task SetHttpAccessorBaseUrlInScopeAsync(ShellScope scope)
    {
        var siteService = scope.ServiceProvider.GetService<ISiteService>();
        if (siteService != null)
        {
            try
            {
                _httpContextAccessor.HttpContext.SetBaseUrl((await siteService.GetSiteSettingsAsync()).BaseUrl);
            }
            catch (Exception exception) when (!exception.IsFatal())
            {
                _logger.LogError(
                    exception,
                    "Error while getting the base url from the site settings of the tenant '{TenantName}'.",
                    scope.ShellContext.Settings.Name);
            }
        }
    }

    private ITenantHostedService GetLoadedServiceByType(string shellName, Type implementationType) =>
        _startedServices.GetValueOrDefault(shellName)
            ?.Keys
            ?.FirstOrDefault(hostedService => hostedService.GetType() == implementationType);

    private async Task StartOrStopServiceByTypeAsync(string shellName, Type implementationType, bool start)
    {
        var hostedService = GetLoadedServiceByType(shellName, implementationType)
            ?? throw new ArgumentException($"The hosted service of type '{implementationType.FullName}' is not loaded.");
        var serviceStatus = _startedServices[shellName][hostedService];
        if (start && serviceStatus.IsStarted)
        {
            throw new InvalidOperationException($"The hosted service of type '{implementationType.FullName}' is already running.");
        }

        if (!start && !serviceStatus.IsStarted)
        {
            throw new InvalidOperationException($"The hosted service of type '{implementationType.FullName}' is not running.");
        }

        try
        {
            if (start)
            {
                await hostedService.StartAsync(CancellationToken.None);
            }
            else
            {
                await hostedService.StopAsync(CancellationToken.None);
            }

            SetSuccessStatus(serviceStatus, isStarted: start);
        }
        catch (Exception exception)
        {
            SetErrorStatus(serviceStatus, exception);

            throw;
        }
    }

    private static void SetSuccessStatus(TenantHostedServiceStatus status, bool isStarted)
    {
        status.IsStarted = isStarted;
        status.HasError = false;
        status.LastError = null;
    }

    private static void SetErrorStatus(TenantHostedServiceStatus status, Exception exception)
    {
        status.HasError = true;
        status.LastError = exception;
    }

    #region ITenantHostedServiceManager implementations

    IEnumerable<ITenantHostedServiceStatus> ITenantHostedServiceManager.GetLoadedServices() =>
        _startedServices.GetValueOrDefault(ShellScope.Current.ShellContext.Settings.Name)
            ?.Values
            ?.ToList()
            ?? Enumerable.Empty<ITenantHostedServiceStatus>();

    Task ITenantHostedServiceManager.StartServiceAsync(Type implementationType) =>
        StartOrStopServiceByTypeAsync(ShellScope.Current.ShellContext.Settings.Name, implementationType, start: true);

    Task ITenantHostedServiceManager.StopServiceAsync(Type implementationType) =>
        StartOrStopServiceByTypeAsync(ShellScope.Current.ShellContext.Settings.Name, implementationType, start: false);

    #endregion
}
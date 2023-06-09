using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell;
using OrchardCore.Settings;
using System;
using System.Threading;
using System.Threading.Tasks;
using VisusCore.AidStack.Extensions;
using VisusCore.TenantHostedService.Core.Services;

namespace VisusCore.TenantHostedService.Samples.Services;

public class SampleBackgroundScopedService : TenantBackgroundScopedService
{
    private readonly ShellSettings _shellSettings;
    private readonly ISiteService _siteService;
    private readonly ISampleScopedService _sampleScopedService;
    private readonly ILogger<SampleBackgroundScopedService> _logger;

    public SampleBackgroundScopedService(
        ShellSettings shellSettings,
        ISiteService siteService,
        ISampleScopedService sampleScopedService,
        ILogger<SampleBackgroundScopedService> logger)
    {
        _shellSettings = shellSettings;
        _siteService = siteService;
        _sampleScopedService = sampleScopedService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var site = await _siteService.GetSiteSettingsAsync();
        _logger.LogInformation(
            "Sample scoped background service started on tenant '{TenantName}' for site '{SiteName}'.",
            _shellSettings.Name,
            site.SiteName);

        while (!await stoppingToken.WaitAsync(TimeSpan.FromSeconds(1)))
        {
            _logger.LogInformation(
                "Sample scoped background service activity on tenant '{TenantName}' for site '{SiteName}'.",
                _shellSettings.Name,
                site.SiteName);

            await _sampleScopedService.DoSomethingAsync(stoppingToken);
        }

        _logger.LogInformation(
            "Sample scoped background service stopped on tenant '{TenantName}' for site '{SiteName}'.",
            _shellSettings.Name,
            site.SiteName);
    }
}

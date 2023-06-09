using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisusCore.TenantHostedService.Abstractions.Models;

namespace VisusCore.TenantHostedService.Abstractions.Services;

/// <summary>
/// Service to manage tenant hosted services.
/// </summary>
public interface ITenantHostedServiceManager
{
    /// <summary>
    /// Returns all loaded services.
    /// </summary>
    IEnumerable<ITenantHostedServiceStatus> GetLoadedServices();

    /// <summary>
    /// Starts a service given by the <paramref name="implementationType"/>.
    /// </summary>
    Task StopServiceAsync(Type implementationType);

    /// <summary>
    /// Stops a service given by the <paramref name="implementationType"/>.
    /// </summary>
    Task StartServiceAsync(Type implementationType);

    /// <summary>
    /// Synchronize the loaded services with the dependency container.
    /// </summary>
    Task UpdateServicesAsync();

    /// <summary>
    /// Returns the hosted service of the given type.
    /// </summary>
    /// <typeparam name="TTenantHostedService">Service type.</typeparam>
    TTenantHostedService GetHostedService<TTenantHostedService>()
        where TTenantHostedService : class, ITenantHostedService;

    /// <summary>
    /// Returns the hosted scoped service of the given type.
    /// </summary>
    /// <typeparam name="TTenantHostedScopedService">Service type.</typeparam>
    TTenantHostedScopedService GetHostedScopedService<TTenantHostedScopedService>()
        where TTenantHostedScopedService : class, ITenantHostedScopedService;
}

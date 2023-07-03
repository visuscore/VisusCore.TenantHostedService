namespace VisusCore.TenantHostedService.Abstractions.Services;

/// <summary>
/// Provides access to a tenant hosted service.
/// </summary>
/// <typeparam name="TTenantHostedService">Service type.</typeparam>
public interface ITenantHostedServiceAccessor<out TTenantHostedService>
    where TTenantHostedService : ITenantHostedService
{
    /// <summary>
    /// Gets the tenant hosted service instance.
    /// </summary>
    TTenantHostedService Service { get; }
}

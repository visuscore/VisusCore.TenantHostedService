namespace VisusCore.TenantHostedService.Abstractions.Services;

/// <summary>
/// Provides access to a tenant hosted scoped service.
/// </summary>
/// <typeparam name="TTenantHostedScopedService">Service type.</typeparam>
public interface ITenantHostedScopedServiceAccessor<out TTenantHostedScopedService>
    where TTenantHostedScopedService : ITenantHostedScopedService
{
    /// <summary>
    /// Gets the tenant hosted service instance.
    /// </summary>
    TTenantHostedScopedService Service { get; }
}

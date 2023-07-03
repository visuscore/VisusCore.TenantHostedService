using VisusCore.TenantHostedService.Abstractions.Services;

namespace VisusCore.TenantHostedService.Core.Services;

public class TenantHostedScopedServiceAccessor<TTenantHostedScopedService>
    : ITenantHostedScopedServiceAccessor<TTenantHostedScopedService>
    where TTenantHostedScopedService : class, ITenantHostedScopedService
{
    private readonly ITenantHostedServiceManager _serviceManager;

    public TenantHostedScopedServiceAccessor(ITenantHostedServiceManager serviceManager) =>
        _serviceManager = serviceManager;

    public TTenantHostedScopedService Service => _serviceManager.GetHostedScopedService<TTenantHostedScopedService>();
}

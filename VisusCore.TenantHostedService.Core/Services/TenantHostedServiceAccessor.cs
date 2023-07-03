using VisusCore.TenantHostedService.Abstractions.Services;

namespace VisusCore.TenantHostedService.Core.Services;

public class TenantHostedServiceAccessor<TTenantHostedService> : ITenantHostedServiceAccessor<TTenantHostedService>
    where TTenantHostedService : class, ITenantHostedService
{
    private readonly ITenantHostedServiceManager _serviceManager;

    public TenantHostedServiceAccessor(ITenantHostedServiceManager serviceManager) =>
        _serviceManager = serviceManager;

    public TTenantHostedService Service => _serviceManager.GetHostedService<TTenantHostedService>();
}

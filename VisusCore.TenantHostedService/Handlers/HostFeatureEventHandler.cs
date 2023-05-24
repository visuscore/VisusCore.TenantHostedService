using OrchardCore.Environment.Extensions.Features;
using OrchardCore.Environment.Shell;
using System.Threading.Tasks;
using VisusCore.TenantHostedService.Abstractions.Services;

namespace VisusCore.TenantHostedService.Handlers;

public class HostFeatureEventHandler : IFeatureEventHandler
{
    private readonly ITenantHostedServiceManager _serviceManager;

    public HostFeatureEventHandler(ITenantHostedServiceManager serviceManager) =>
        _serviceManager = serviceManager;

    public Task InstallingAsync(IFeatureInfo feature) => Task.CompletedTask;

    public Task InstalledAsync(IFeatureInfo feature) => Task.CompletedTask;

    public Task EnablingAsync(IFeatureInfo feature) => Task.CompletedTask;

    public Task EnabledAsync(IFeatureInfo feature) => _serviceManager.UpdateServicesAsync();

    public Task DisablingAsync(IFeatureInfo feature) => Task.CompletedTask;

    public Task DisabledAsync(IFeatureInfo feature) => _serviceManager.UpdateServicesAsync();

    public Task UninstallingAsync(IFeatureInfo feature) => Task.CompletedTask;

    public Task UninstalledAsync(IFeatureInfo feature) => Task.CompletedTask;
}

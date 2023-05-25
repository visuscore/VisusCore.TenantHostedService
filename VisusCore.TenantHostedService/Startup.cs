using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using VisusCore.TenantHostedService.Handlers;
using VisusCore.TenantHostedService.Navigation;
using VisusCore.TenantHostedService.Permissions;

namespace VisusCore.TenantHostedService;

[Feature(Constants.FeatureIds.Host)]
public class StartupHost : StartupBase
{
    public override void ConfigureServices(IServiceCollection services) =>
        services
            .AddScoped<IFeatureEventHandler, HostFeatureEventHandler>();
}

[Feature(Constants.FeatureIds.Loader)]
public class StartupLoader : StartupBase
{
    public override void ConfigureServices(IServiceCollection services) =>
        services
            .AddScoped<IPermissionProvider, LoaderPermissions>()
            .AddScoped<INavigationProvider, TenantHostedServiceAdminMenu>();
}

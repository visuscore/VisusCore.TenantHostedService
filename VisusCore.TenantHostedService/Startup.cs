using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using VisusCore.TenantHostedService.Navigation;
using VisusCore.TenantHostedService.Permissions;

namespace VisusCore.TenantHostedService;

[Feature(Constants.FeatureIds.Loader)]
public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services) =>
        services
            .AddScoped<IPermissionProvider, LoaderPermissions>()
            .AddScoped<INavigationProvider, TenantHostedServiceAdminMenu>();
}

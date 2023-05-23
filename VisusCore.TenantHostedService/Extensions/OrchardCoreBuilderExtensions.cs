using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using VisusCore.TenantHostedService.Abstractions.Services;
using VisusCore.TenantHostedService.Services;

namespace VisusCore.TenantHostedService.Extensions;

public static class OrchardCoreBuilderExtensions
{
    public static OrchardCoreBuilder AddTenantHostedService(this OrchardCoreBuilder builder)
    {
        builder.ApplicationServices.AddHostedService<TenantHostedServiceManager>()
            .AddSingleton<ITenantHostedServiceManager>(services =>
                services.GetServices<IHostedService>().First(
                    service => service is TenantHostedServiceManager) as TenantHostedServiceManager);

        return builder;
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrchardCore.Environment.Shell.Descriptor.Models;
using System;
using System.Linq;
using VisusCore.TenantHostedService.Abstractions.Services;
using VisusCore.TenantHostedService.Constants;
using VisusCore.TenantHostedService.Services;

namespace VisusCore.TenantHostedService.Extensions;

public static class OrchardCoreBuilderExtensions
{
    public static OrchardCoreBuilder AddTenantHostedService(this OrchardCoreBuilder builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.ApplicationServices.AddHostedService<TenantHostedServiceManager>()
            .AddSingleton<ITenantHostedServiceManager>(services =>
                services.GetServices<IHostedService>().First(
                    service => service is TenantHostedServiceManager) as TenantHostedServiceManager)
            .AddTransient(serviceProvider => new ShellFeature(FeatureIds.Host, alwaysEnabled: true));

        return builder;
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VisusCore.TenantHostedService.Abstractions.Services;
using VisusCore.TenantHostedService.Core.Services;

namespace VisusCore.TenantHostedService.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTenantHostedService<TTenantHostedService>(this IServiceCollection services)
        where TTenantHostedService : class, ITenantHostedService
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ITenantHostedService, TTenantHostedService>());
        services.AddScoped<
            ITenantHostedServiceAccessor<TTenantHostedService>,
            TenantHostedServiceAccessor<TTenantHostedService>>();

        return services;
    }

    public static IServiceCollection AddTenantHostedScopedService<TTenantHostedService>(this IServiceCollection services)
        where TTenantHostedService : class, ITenantHostedScopedService
    {
        services.TryAddEnumerable(ServiceDescriptor.Scoped<ITenantHostedScopedService, TTenantHostedService>());
        services.AddScoped<
            ITenantHostedScopedServiceAccessor<TTenantHostedService>,
            TenantHostedScopedServiceAccessor<TTenantHostedService>>();

        return services;
    }
}

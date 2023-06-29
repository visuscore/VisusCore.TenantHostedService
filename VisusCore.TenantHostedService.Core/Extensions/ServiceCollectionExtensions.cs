using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using VisusCore.TenantHostedService.Abstractions.Services;

namespace VisusCore.TenantHostedService.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTenantHostedService<TTenantHostedService>(this IServiceCollection services)
        where TTenantHostedService : class, ITenantHostedService
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ITenantHostedService, TTenantHostedService>());

        return services;
    }

    public static IServiceCollection AddTenantHostedService<THostedService>(
        this IServiceCollection services,
        Func<IServiceProvider, THostedService> implementationFactory)
        where THostedService : class, ITenantHostedService
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ITenantHostedService>(implementationFactory));

        return services;
    }

    public static IServiceCollection AddScopedTenantHostedService<TTenantHostedService>(this IServiceCollection services)
        where TTenantHostedService : class, ITenantHostedScopedService
    {
        services.TryAddEnumerable(ServiceDescriptor.Scoped<ITenantHostedScopedService, TTenantHostedService>());

        return services;
    }

    public static IServiceCollection AddScopedTenantHostedService<THostedService>(
        this IServiceCollection services,
        Func<IServiceProvider, THostedService> implementationFactory)
        where THostedService : class, ITenantHostedScopedService
    {
        services.TryAddEnumerable(ServiceDescriptor.Scoped<ITenantHostedScopedService>(implementationFactory));

        return services;
    }
}

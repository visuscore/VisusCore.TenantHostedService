using System;
using System.Linq;
using VisusCore.TenantHostedService.Abstractions.Models;
using VisusCore.TenantHostedService.Abstractions.Services;

namespace VisusCore.TenantHostedService.Extensions;

public static class TenantHostedServiceManagerExtensions
{
    public static ITenantHostedServiceStatus GetStatusByTypeName(
        this ITenantHostedServiceManager serviceManager,
        string typeName)
    {
        if (serviceManager is null)
        {
            throw new ArgumentNullException(nameof(serviceManager));
        }

        return serviceManager.GetLoadedServices()
            .First(status => status.ImplementationType.FullName.Equals(typeName, StringComparison.OrdinalIgnoreCase));
    }
}

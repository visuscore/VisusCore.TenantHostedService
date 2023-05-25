using OrchardCore.Security.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisusCore.TenantHostedService.Permissions;

public class LoaderPermissions : IPermissionProvider
{
    public static readonly Permission ManageTenantHostedServices = new(
        nameof(ManageTenantHostedServices),
        "Manage tenant hosted services");

    public Task<IEnumerable<Permission>> GetPermissionsAsync() =>
        Task.FromResult(new[]
        {
            ManageTenantHostedServices,
        }
        .AsEnumerable());

    public IEnumerable<PermissionStereotype> GetDefaultStereotypes() =>
        new[]
        {
            new PermissionStereotype
            {
                Name = "Administrator",
                Permissions = new[] { ManageTenantHostedServices },
            },
        };
}

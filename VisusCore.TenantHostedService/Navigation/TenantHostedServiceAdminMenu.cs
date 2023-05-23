using Microsoft.Extensions.Localization;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;
using System;
using System.Threading.Tasks;
using VisusCore.TenantHostedService.Constants;
using VisusCore.TenantHostedService.Controllers;
using VisusCore.TenantHostedService.Permissions;

namespace VisusCore.TenantHostedService.Navigation;

public class TenantHostedServiceAdminMenu : INavigationProvider
{
    private readonly IStringLocalizer T;

    public TenantHostedServiceAdminMenu(IStringLocalizer<TenantHostedServiceAdminMenu> localizer) =>
        T = localizer;

    public Task BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
        {
            return Task.CompletedTask;
        }

        builder
            .Add(T["Configuration"], configuration => configuration
                .Add(T["Tasks"], tasks => tasks
                    .Add(T["Tenant Hosted Services"], tenantHostedServices => tenantHostedServices
                        .Action(
                            nameof(TenantHostedServiceController.Index),
                            typeof(TenantHostedServiceController).ControllerName(),
                            new { area = FeatureIds.Module })
                        .Permission(LoaderPermissions.ManageTenantHostedServices)
                        .LocalNav()
                    )
                )
            );

        return Task.CompletedTask;
    }
}

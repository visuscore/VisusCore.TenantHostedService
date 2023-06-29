using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;
using OrchardCore.Modules;
using System.Linq;
using System.Threading.Tasks;
using VisusCore.TenantHostedService.Abstractions.Services;
using VisusCore.TenantHostedService.Constants;
using VisusCore.TenantHostedService.Extensions;
using VisusCore.TenantHostedService.Permissions;
using VisusCore.TenantHostedService.ViewModels;

namespace VisusCore.TenantHostedService.Controllers;

[Admin]
[Feature(FeatureIds.Loader)]
public class TenantHostedServiceController : Controller
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ITenantHostedServiceManager _serviceManager;

    public TenantHostedServiceController(
        IAuthorizationService authorizationService,
        ITenantHostedServiceManager serviceManager)
    {
        _authorizationService = authorizationService;
        _serviceManager = serviceManager;
    }

    public async Task<IActionResult> Index()
    {
        if (!await _authorizationService.AuthorizeAsync(User, LoaderPermissions.ManageTenantHostedServices))
        {
            return NotFound();
        }

        return View(new TenantHostedServiceIndexViewModel
        {
            Services = _serviceManager.GetLoadedServices()
                .Select(service => new TenantHostedServiceItemViewModel
                {
                    HasError = service.HasError,
                    IsScoped = service.IsScoped,
                    IsStarted = service.IsStarted,
                    IsCompleted = service.IsCompleted,
                    TypeName = service.ImplementationType.FullName,
                }),
        });
    }

    public async Task<IActionResult> Start(string typeName)
    {
        if (!await _authorizationService.AuthorizeAsync(User, LoaderPermissions.ManageTenantHostedServices))
        {
            return NotFound();
        }

        if (_serviceManager.GetStatusByTypeName(typeName) is not { } serviceStatus)
        {
            return NotFound();
        }

        await _serviceManager.StartServiceAsync(serviceStatus.ImplementationType);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Stop(string typeName)
    {
        if (!await _authorizationService.AuthorizeAsync(User, LoaderPermissions.ManageTenantHostedServices))
        {
            return NotFound();
        }

        if (_serviceManager.GetStatusByTypeName(typeName) is not { } serviceStatus)
        {
            return NotFound();
        }

        await _serviceManager.StopServiceAsync(serviceStatus.ImplementationType);

        return RedirectToAction(nameof(Index));
    }
}

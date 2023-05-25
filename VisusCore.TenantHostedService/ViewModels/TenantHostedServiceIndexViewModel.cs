using System.Collections.Generic;

namespace VisusCore.TenantHostedService.ViewModels;

public class TenantHostedServiceIndexViewModel
{
    public IEnumerable<TenantHostedServiceItemViewModel> Services { get; set; }
}

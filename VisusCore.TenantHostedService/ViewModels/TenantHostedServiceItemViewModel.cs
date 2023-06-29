namespace VisusCore.TenantHostedService.ViewModels;

public class TenantHostedServiceItemViewModel
{
    public string TypeName { get; set; }
    public bool IsScoped { get; set; }
    public bool IsStarted { get; set; }
    public bool IsCompleted { get; set; }
    public bool HasError { get; set; }
}

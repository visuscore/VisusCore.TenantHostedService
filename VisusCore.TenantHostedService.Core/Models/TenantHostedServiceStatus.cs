using System;
using VisusCore.TenantHostedService.Abstractions.Models;

namespace VisusCore.TenantHostedService.Core.Models;

public class TenantHostedServiceStatus : ITenantHostedServiceStatus
{
    public Type ImplementationType { get; set; }
    public bool IsScoped { get; set; }
    public bool IsStarted { get; set; }
    public bool IsCompleted { get; set; }
    public bool HasError { get; set; }
    public Exception LastError { get; set; }
}

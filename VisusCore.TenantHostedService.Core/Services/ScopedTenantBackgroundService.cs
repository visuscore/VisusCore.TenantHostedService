using Microsoft.Extensions.Hosting;
using VisusCore.TenantHostedService.Abstractions.Services;

namespace VisusCore.TenantHostedService.Core.Services;

public abstract class ScopedTenantBackgroundService : BackgroundService, ITenantHostedScopedService
{
}

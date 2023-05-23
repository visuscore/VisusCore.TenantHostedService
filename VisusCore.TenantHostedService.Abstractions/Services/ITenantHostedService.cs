using Microsoft.Extensions.Hosting;

namespace VisusCore.TenantHostedService.Abstractions.Services;

/// <summary>
/// Interface for hosted services running on tenant level.
/// </summary>
public interface ITenantHostedService : IHostedService
{
}

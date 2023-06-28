using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace VisusCore.TenantHostedService.Abstractions.Services;

/// <summary>
/// Interface for hosted services running on tenant level in a service scope.
/// </summary>
public interface IScopedTenantHostedService : IHostedService
{
    /// <summary>
    /// Gets the Task that executes the background operation.
    /// </summary>
    Task ExecuteTask { get; }
}

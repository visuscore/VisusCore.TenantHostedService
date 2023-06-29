using System.Threading;
using System.Threading.Tasks;

namespace VisusCore.TenantHostedService.Samples.Services;

/// <summary>
/// Sample scoped service.
/// </summary>
public interface ISampleScopedService
{
    /// <summary>
    /// Does something.
    /// </summary>
    Task DoSomethingAsync(CancellationToken cancellationToken = default);
}

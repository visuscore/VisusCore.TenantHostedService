using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VisusCore.AidStack.Extensions;

namespace VisusCore.TenantHostedService.Samples.Services;

public class SampleScopedService : ISampleScopedService, IDisposable
{
    private readonly ILogger _logger;
    private bool _disposed;

    public SampleScopedService(ILogger<SampleScopedService> logger) =>
        _logger = logger;

    public async Task DoSomethingAsync(CancellationToken cancellationToken = default)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(SampleScopedService));

        await cancellationToken.WaitAsync(TimeSpan.FromSeconds(1));

        _logger.LogInformation("Sample scoped service did something.");
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

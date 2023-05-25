using System;

namespace VisusCore.TenantHostedService.Abstractions.Models;

/// <summary>
/// Interface to get the status of a tenant hosted service.
/// </summary>
public interface ITenantHostedServiceStatus
{
    /// <summary>
    /// Gets the implementation type of the hosted service.
    /// </summary>
    Type ImplementationType { get; }

    /// <summary>
    /// Gets a value indicating whether if the hosted service is started.
    /// </summary>
    bool IsStarted { get; }

    /// <summary>
    /// Gets a value indicating whether if the hosted service has an error.
    /// </summary>
    bool HasError { get; }

    /// <summary>
    /// Gets the last error of the hosted service.
    /// </summary>
    Exception LastError { get; }
}

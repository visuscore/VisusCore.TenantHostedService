using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell;
using OrchardCore.Settings;
using System.Threading;
using System.Threading.Tasks;
using VisusCore.TenantHostedService.Core.Services;

namespace VisusCore.TenantHostedService.Samples.Services;

public class SampleBackgroundService : TenantBackgroundService
{
    private readonly ShellSettings _shellSettings;
    private readonly ISiteService _siteService;
    private readonly ILogger<SampleBackgroundService> _logger;

    public SampleBackgroundService(
        ShellSettings shellSettings,
        ISiteService siteService,
        ILogger<SampleBackgroundService> logger)
    {
        _shellSettings = shellSettings;
        _siteService = siteService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var site = await _siteService.GetSiteSettingsAsync();
        _logger.LogInformation(
            "Sample background service started on tenant '{TenantName}' for site '{SiteName}'.",
            _shellSettings.Name,
            site.SiteName);

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (TaskCanceledException)
        {
            // This happens when the service is stopped. Nothing to do here.
        }

        _logger.LogInformation(
            "Sample background service stopped on tenant '{TenantName}' for site '{SiteName}'.",
            _shellSettings.Name,
            site.SiteName);
    }
}

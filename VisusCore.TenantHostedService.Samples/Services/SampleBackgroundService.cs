using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Shell;
using OrchardCore.Settings;
using System;
using System.Threading;
using System.Threading.Tasks;
using VisusCore.AidStack.Extensions;
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

        while (!await stoppingToken.WaitAsync(TimeSpan.FromSeconds(1)))
        {
            _logger.LogInformation(
                "Sample background service activity on tenant '{TenantName}' for site '{SiteName}'.",
                _shellSettings.Name,
                site.SiteName);
        }

        _logger.LogInformation(
            "Sample background service stopped on tenant '{TenantName}' for site '{SiteName}'.",
            _shellSettings.Name,
            site.SiteName);
    }
}

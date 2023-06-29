using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using VisusCore.TenantHostedService.Core.Extensions;
using VisusCore.TenantHostedService.Samples.Services;

namespace VisusCore.TenantHostedService.Samples;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services) =>
        services
            .AddTenantHostedService<SampleBackgroundService>()
            .AddScopedTenantHostedService<SampleBackgroundScopedService>()
            .AddScoped<ISampleScopedService, SampleScopedService>();
}

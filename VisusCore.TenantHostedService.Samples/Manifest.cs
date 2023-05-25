using OrchardCore.Modules.Manifest;
using VisusCore.TenantHostedService.Constants;

[assembly: Module(
    Name = "Tenant Hosted Service - Samples",
    Author = "VisusCore",
    Version = "0.0.1",
    Description = "Samples for Tenant Hosted Service.",
    Website = "https://github.com/visuscore/VisusCore.TenantHostedService",
    Category = "Samples",
    Dependencies = new[]
    {
        FeatureIds.Loader,
    }
)]

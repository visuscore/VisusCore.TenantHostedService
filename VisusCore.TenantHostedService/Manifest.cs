using OrchardCore.Modules.Manifest;
using VisusCore.TenantHostedService.Constants;

[assembly: Module(
    Name = "Tenant Hosted Service",
    Author = "VisusCore",
    Version = "0.0.1",
    Description = "Module for starting HostedService in a tenant level.",
    Website = "https://github.com/visuscore/VisusCore.TenantHostedService"
)]

[assembly: Feature(
    Id = FeatureIds.Host,
    Name = "Tenant Hosted Service Host",
    Category = "Infrastructure"
)]

[assembly: Feature(
    Id = FeatureIds.Loader,
    Name = "Tenant Hosted Service Loader",
    Category = "Infrastructure"
)]

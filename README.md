# Tenant Hosted Service

## About

Orchard Core module that allows tenants to host their own services similar to [Background tasks with hosted services in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-6.0&tabs=visual-studio) but on the tenant level.

## Usage

To incorporate the tenant hosted service infrastructure into Orchard Core, simply utilize `OrchardCoreBuilderExtensions.AddTenantHostedService`.

```diff
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VisusCore.TenantHostedService.Extensions;

var builder = WebApplication.CreateBuilder(args);

- builder.Services.AddOrchardCms();
+ builder.Services.AddOrchardCms(builder => builder.AddTenantHostedService());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseOrchardCore();

app.Run();
```

## Features

### Tenant Hosted Service Host

The purpose of this is to identify whether a feature is enabled or disabled for a tenant. It will be enabled for every tenant through `OrchardCoreBuilderExtensions.AddTenantHostedService`.

### Tenant Hosted Service Loader

The objective of this is to designate the tenant for the hosted service manager to load the hosted services specific to that tenant.

## Samples

If you're interested in observing the entire functionality in practice, take a look at the [Samples project](VisusCore.TenantHostedService.Samples/README.md).

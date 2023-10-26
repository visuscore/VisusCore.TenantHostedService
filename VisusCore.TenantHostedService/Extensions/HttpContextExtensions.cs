using Microsoft.AspNetCore.Http;
using System;

namespace VisusCore.TenantHostedService.Extensions;

public static class HttpContextExtensions
{
    public static void SetBaseUrl(this HttpContext context, string baseUrl)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri))
        {
            context.Request.Scheme = uri.Scheme;
            context.Request.Host = new HostString(uri.Host, uri.Port);
            context.Request.PathBase = uri.AbsolutePath;

            if (!string.IsNullOrWhiteSpace(uri.Query))
            {
                context.Request.QueryString = new QueryString(uri.Query);
            }
        }
    }
}

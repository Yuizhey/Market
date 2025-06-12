using Market.Application.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Market.Application.Localization;

public static class LocalizationExtensions
{
    public static IApplicationBuilder UseLocalization(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LocalizationMiddleware>();
    }
} 
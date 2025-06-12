using System.Globalization;
using Market.Application.Localization;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Middleware;

public class LocalizationMiddleware
{
    private const string CultureCookieName = "user_culture";
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var culture = GetCultureFromCookie(context);
        
        var cultureInfo = new CultureInfo(culture);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
        
        if (!context.Request.Cookies.ContainsKey(CultureCookieName))
        {
            context.Response.Cookies.Append(CultureCookieName, culture, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.Now.AddYears(1)
            });
        }

        await _next(context);
    }

    private string GetCultureFromCookie(HttpContext context)
    {
        var culture = context.Request.Cookies[CultureCookieName];
        
        if (string.IsNullOrEmpty(culture) || !SupportedCultures.Supported.Contains(culture))
        {
            return SupportedCultures.DefaultCulture;
        }

        return culture;
    }
}
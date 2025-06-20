using Microsoft.AspNetCore.Mvc;
using Market.Application.Localization;

namespace Market.MVC.Controllers;

public class LocalizationController : Controller
{
    [HttpGet]
    public IActionResult SetLanguage(string culture, string returnUrl = "/")
    {
        if (!SupportedCultures.Supported.Contains(culture))
            culture = SupportedCultures.DefaultCulture;

        Response.Cookies.Append(
            "user_culture",
            culture,
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            });

        return LocalRedirect(returnUrl);
    }
} 
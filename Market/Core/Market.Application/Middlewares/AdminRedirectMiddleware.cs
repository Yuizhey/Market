using Microsoft.AspNetCore.Http;

namespace Market.Application.Middlewares;

public class AdminRedirectMiddleware
{
    private readonly RequestDelegate _next;

    public AdminRedirectMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.IsInRole("Admin"))
        {
            var path = context.Request.Path.Value?.ToLower();
            if (path != null && !path.StartsWith("/admin") && !path.StartsWith("/_blazor") && !path.StartsWith("/css") && !path.StartsWith("/js") && !path.StartsWith("/lib") && !path.StartsWith("/images"))
            {
                context.Response.Redirect("/Admin/Contact");
                return;
            }
        }

        await _next(context);
    }
} 
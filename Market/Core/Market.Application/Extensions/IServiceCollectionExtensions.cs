using System.Reflection;
using Market.Application.Middleware;
using Market.Application.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Market.Application.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediator();
    }

    private static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseAdminRedirect(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AdminRedirectMiddleware>();
    }
}
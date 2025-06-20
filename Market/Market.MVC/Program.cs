using Market.Application.Extensions;
using Market.Application.Localization;
using Market.Domain.Extensions;
using Market.Infrastructure.Extensions;
using Market.Infrastructure.Services;
using Market.Persistence.Contexts;
using Market.Persistence.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Minio;
using Microsoft.Extensions.Localization;

namespace Market.MVC;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        })
        .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddSingleton<IMinioClient>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var minioConfig = configuration.GetSection("Minio");
            var client = new MinioClient()
                .WithEndpoint(minioConfig["Endpoint"])
                .WithCredentials(minioConfig["AccessKey"], minioConfig["SecretKey"]);
    
            if (bool.Parse(minioConfig["UseSSL"]))
            {
                client.WithSSL();
            }
    
            return client.Build();
        });
        builder.Services.AddApplicationLayer();
        builder.Services.AddInfrastructureLayer();
        builder.Services.AddPersistenceLayer(builder.Configuration);

        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
        builder.Services.AddControllersWithViews()
            .AddViewLocalization()
            .AddDataAnnotationsLocalization();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/login";
            options.AccessDeniedPath = "/Error/403";
        });
        var app = builder.Build();
        
        
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await RoleInitializer.SeedRolesAsync(services);
            await DataSeeder.SeedAdminAsync(services);
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error/404");
            app.UseHsts();
        }
        
        app.UseStatusCodePagesWithReExecute("/Error/{0}");

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseMiddleware<Market.Application.Middleware.LocalizationMiddleware>();

        var supportedCultures = new[] { "en", "ru" };
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture("ru")
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAdminRedirect();

        app.MapHub<NotificationHub>("/notificationhub");

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
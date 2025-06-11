using Market.Application.Interfaces.Repositories;
using Market.Persistence.Contexts;
using Market.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Market.Persistence.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        services.AddRepositories();
    }

    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services
            .AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
            .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddTransient<IProductRepository, ProductRepository>()
            .AddTransient<ICartRepository, CartRepository>()
            .AddTransient<IUserDescriptionRepository, UserDescriptionRepository>()
            .AddTransient<IAuthorUserDescriptionRepository, AuthorUserDescriptionRepository>()
            .AddTransient<IProductSaleStatisticsRepository, ProductSaleStatisticsRepository>()
            .AddTransient<IPurchaseRepository, PurchaseRepository>()
            .AddScoped<ILikeRepository, LikeRepository>();
    }
}
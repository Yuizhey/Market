using Market.Application.Interfaces.Services;
using Market.FileStorage.Services;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace Market.FileStorage.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddFileStorageLayer(this IServiceCollection collection)
    {
        collection.AddServices();
        collection.AddMinio();
    }
    
    private static void AddServices(this IServiceCollection sericeCollection)
    {
        sericeCollection
            .AddScoped<IFileStorageService, MinioFileStorageService>();
    }
    
    private static void AddMinio(this IServiceCollection services)
    {
        services.AddSingleton(new MinioClient()
            .WithEndpoint("localhost:9000")
            .WithCredentials("minioadmin", "minioadmin")
            .Build());
    }
}
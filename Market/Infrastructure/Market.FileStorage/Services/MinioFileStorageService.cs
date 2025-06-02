using Microsoft.AspNetCore.Http;
using Minio;
using Minio.DataModel.Args;
using Market.Application.Interfaces.Services;

using Minio;
using Minio.DataModel.Args;

public class MinioFileStorageService : IFileStorageService
{
    private readonly MinioClient _minioClient;
    private readonly string _bucketName = "product-images";

    public MinioFileStorageService()
    {
        _minioClient = new MinioClient()
            .WithEndpoint("localhost:9000") // Заменить на свой
            .WithCredentials("minioadmin", "minioadmin") // Заменить на свои креды
            .Build();
    }

    public async Task<string> UploadFileAsync(Stream stream, string fileName, string contentType, long size)
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(_bucketName);
        var exists = await _minioClient.BucketExistsAsync(bucketExistsArgs);
        if (!exists)
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(_bucketName);
            await _minioClient.MakeBucketAsync(makeBucketArgs);
        }

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileName)
            .WithStreamData(stream)
            .WithObjectSize(size)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putObjectArgs);

        return $"http://localhost:9000/{_bucketName}/{fileName}";
    }
}

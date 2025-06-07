using System.IO.Compression;
using Market.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace Market.Infrastructure.Services;

public class MinioService : IMinioService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public MinioService(IMinioClient minioClient, IConfiguration configuration)
    {
        _minioClient = minioClient;
        _bucketName = configuration["Minio:BucketName"] ?? throw new ArgumentNullException("Minio:BucketName configuration is missing");
    }

    public async Task<string> UploadCoverImageAsync(IFormFile file, Guid productId, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("No file provided or file is empty.");
        }
        
        var validContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        if (!validContentTypes.Contains(file.ContentType))
        {
            throw new ArgumentException("Invalid file type. Only JPEG, PNG, GIF, and WEBP are allowed.");
        }
        
        const long maxFileSize = 5 * 1024 * 1024; 
        if (file.Length > maxFileSize)
        {
            throw new ArgumentException("File size exceeds the 5MB limit.");
        }
        
        var bucketExists = await _minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_bucketName), 
            cancellationToken);
        if (!bucketExists)
        {
            await _minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(_bucketName), 
                cancellationToken);
        }
        
        var fileName = $"{productId}_{Path.GetFileName(file.FileName)}";
        var objectName = $"covers/{fileName}";
        
        using (var stream = file.OpenReadStream())
        {
            await _minioClient.PutObjectAsync(
                new PutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(objectName)
                    .WithStreamData(stream)
                    .WithObjectSize(file.Length)
                    .WithContentType(file.ContentType),
                cancellationToken);
        }

        return objectName;
    }

    public async Task DeleteFileAsync(string filePath, CancellationToken cancellationToken)
    {
        await _minioClient.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(filePath),
            cancellationToken);
    }

    public async Task<string> GetCoverImageUrlAsync(string objectName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(objectName))
        {
            throw new ArgumentException("Object name cannot be null or empty.");
        }

        var presignedUrlArgs = new PresignedGetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithExpiry(3600); 

        return await _minioClient.PresignedGetObjectAsync(presignedUrlArgs);
    }
    
    public async Task<List<string>> UploadAdditionalFilesAsync(IFormFile[] files, Guid productId, CancellationToken cancellationToken)
    {
        if (files == null || !files.Any())
        {
            throw new ArgumentException("No files provided.");
        }

        if (files.Length > 10)
        {
            throw new ArgumentException("Cannot upload more than 10 files.");
        }

        const long maxFileSize = 10 * 1024 * 1024; // 10MB limit for additional files
        var filePaths = new List<string>();

        var bucketExists = await _minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_bucketName), 
            cancellationToken);
        if (!bucketExists)
        {
            await _minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(_bucketName), 
                cancellationToken);
        }

        foreach (var file in files)
        {
            if (file == null || file.Length == 0)
            {
                continue; 
            }

            if (file.Length > maxFileSize)
            {
                throw new ArgumentException($"File {file.FileName} exceeds the 10MB limit.");
            }

            var fileName = $"{productId}_{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var objectName = $"additional-files/{fileName}";

            using (var stream = file.OpenReadStream())
            {
                await _minioClient.PutObjectAsync(
                    new PutObjectArgs()
                        .WithBucket(_bucketName)
                        .WithObject(objectName)
                        .WithStreamData(stream)
                        .WithObjectSize(file.Length)
                        .WithContentType(file.ContentType),
                    cancellationToken);
            }

            filePaths.Add(objectName);
        }

        return filePaths;
    }
    
    public async Task<List<string>> GetAdditionalFilesUrlsAsync(List<string> objectNames, CancellationToken cancellationToken)
    {
        if (objectNames == null || !objectNames.Any())
        {
            return new List<string>();
        }

        var urls = new List<string>();
        foreach (var objectName in objectNames)
        {
            if (string.IsNullOrEmpty(objectName))
            {
                continue;
            }

            var presignedUrlArgs = new PresignedGetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithExpiry(3600)
                .WithHeaders(new Dictionary<string, string>
                {
                    { "Content-Disposition", $"attachment; filename=\"{Path.GetFileName(objectName)}\"" }
                });

            var url = await _minioClient.PresignedGetObjectAsync(presignedUrlArgs);
            urls.Add(url);
        }

        return urls;
    }
    
    public async Task<byte[]> CreateZipFromFilesAsync(List<string> objectNames, CancellationToken cancellationToken)
    {
        if (objectNames == null || !objectNames.Any())
        {
            return Array.Empty<byte>();
        }

        using var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            using var httpClient = new HttpClient();
            foreach (var objectName in objectNames)
            {
                if (string.IsNullOrEmpty(objectName))
                {
                    continue;
                }

                var presignedUrlArgs = new PresignedGetObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(objectName)
                    .WithExpiry(3600);

                var presignedUrl = await _minioClient.PresignedGetObjectAsync(presignedUrlArgs);
                var fileStream = await httpClient.GetStreamAsync(presignedUrl, cancellationToken);
                var entry = archive.CreateEntry(Path.GetFileName(objectName));

                using var entryStream = entry.Open();
                await fileStream.CopyToAsync(entryStream, cancellationToken);
            }
        }

        return memoryStream.ToArray();
    }
}
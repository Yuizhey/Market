using System.IO.Compression;
using Market.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace Market.Infrastructure.Services;

public class MinioService : IMinioService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;
    private readonly ILogger<MinioService> _logger;

    public MinioService(IMinioClient minioClient, IConfiguration configuration, ILogger<MinioService> logger)
    {
        _minioClient = minioClient;
        _bucketName = configuration["Minio:BucketName"] ?? throw new ArgumentNullException("Minio:BucketName configuration is missing");
        _logger = logger;
        _logger.LogInformation("MinioService инициализирован. Bucket: {BucketName}", _bucketName);
    }

    public async Task<string> UploadCoverImageAsync(IFormFile file, Guid productId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Начало загрузки обложки для продукта {ProductId}. Файл: {FileName}, Размер: {Size} байт", 
            productId, file?.FileName, file?.Length);

        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("Попытка загрузки пустого файла для продукта {ProductId}", productId);
            throw new ArgumentException("No file provided or file is empty.");
        }
        
        var validContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        if (!validContentTypes.Contains(file.ContentType))
        {
            _logger.LogWarning("Недопустимый тип файла {ContentType} для продукта {ProductId}", file.ContentType, productId);
            throw new ArgumentException("Invalid file type. Only JPEG, PNG, GIF, and WEBP are allowed.");
        }
        
        const long maxFileSize = 5 * 1024 * 1024; 
        if (file.Length > maxFileSize)
        {
            _logger.LogWarning("Превышен размер файла {Size} байт для продукта {ProductId}", file.Length, productId);
            throw new ArgumentException("File size exceeds the 5MB limit.");
        }
        
        var bucketExists = await _minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_bucketName), 
            cancellationToken);
        if (!bucketExists)
        {
            _logger.LogInformation("Создание бакета {BucketName}", _bucketName);
            await _minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(_bucketName), 
                cancellationToken);
        }
        
        var fileName = $"{productId}_{Path.GetFileName(file.FileName)}";
        var objectName = $"covers/{fileName}";
        
        try
        {
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
            _logger.LogInformation("Обложка успешно загружена для продукта {ProductId}. Путь: {ObjectName}", 
                productId, objectName);
            return objectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при загрузке обложки для продукта {ProductId}", productId);
            throw;
        }
    }

    public async Task DeleteFileAsync(string filePath, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Начало удаления файла {FilePath}", filePath);
        
        try
        {
            await _minioClient.RemoveObjectAsync(
                new RemoveObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(filePath),
                cancellationToken);
            _logger.LogInformation("Файл успешно удален: {FilePath}", filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении файла {FilePath}", filePath);
            throw;
        }
    }

    public async Task<string> GetCoverImageUrlAsync(string objectName, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение URL для обложки {ObjectName}", objectName);
        
        if (string.IsNullOrEmpty(objectName))
        {
            _logger.LogWarning("Попытка получить URL для пустого имени объекта");
            throw new ArgumentException("Object name cannot be null or empty.");
        }

        try
        {
            var presignedUrlArgs = new PresignedGetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithExpiry(3600); 

            var url = await _minioClient.PresignedGetObjectAsync(presignedUrlArgs);
            _logger.LogInformation("URL успешно получен для обложки {ObjectName}", objectName);
            return url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении URL для обложки {ObjectName}", objectName);
            throw;
        }
    }
    
    public async Task<List<string>> UploadAdditionalFilesAsync(IFormFile[] files, Guid productId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Начало загрузки дополнительных файлов для продукта {ProductId}. Количество файлов: {Count}", 
            productId, files?.Length ?? 0);

        if (files == null || !files.Any())
        {
            _logger.LogWarning("Попытка загрузки пустого массива файлов для продукта {ProductId}", productId);
            throw new ArgumentException("No files provided.");
        }

        if (files.Length > 10)
        {
            _logger.LogWarning("Превышено максимальное количество файлов ({Count}) для продукта {ProductId}", 
                files.Length, productId);
            throw new ArgumentException("Cannot upload more than 10 files.");
        }

        const long maxFileSize = 10 * 1024 * 1024;
        var filePaths = new List<string>();

        var bucketExists = await _minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_bucketName), 
            cancellationToken);
        if (!bucketExists)
        {
            _logger.LogInformation("Создание бакета {BucketName}", _bucketName);
            await _minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(_bucketName), 
                cancellationToken);
        }

        foreach (var file in files)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("Пропуск пустого файла в массиве для продукта {ProductId}", productId);
                continue;
            }

            if (file.Length > maxFileSize)
            {
                _logger.LogWarning("Превышен размер файла {FileName} ({Size} байт) для продукта {ProductId}", 
                    file.FileName, file.Length, productId);
                throw new ArgumentException($"File {file.FileName} exceeds the 10MB limit.");
            }

            var fileName = $"{productId}_{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var objectName = $"additional-files/{fileName}";

            try
            {
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
                _logger.LogInformation("Дополнительный файл успешно загружен: {ObjectName}", objectName);
                filePaths.Add(objectName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке дополнительного файла {FileName} для продукта {ProductId}", 
                    file.FileName, productId);
                throw;
            }
        }

        _logger.LogInformation("Загрузка дополнительных файлов завершена для продукта {ProductId}. Загружено файлов: {Count}", 
            productId, filePaths.Count);
        return filePaths;
    }
    
    public async Task<List<string>> GetAdditionalFilesUrlsAsync(List<string> objectNames, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение URL для {Count} дополнительных файлов", objectNames?.Count ?? 0);
        
        if (objectNames == null || !objectNames.Any())
        {
            _logger.LogWarning("Попытка получить URL для пустого списка файлов");
            return new List<string>();
        }

        var urls = new List<string>();
        foreach (var objectName in objectNames)
        {
            if (string.IsNullOrEmpty(objectName))
            {
                _logger.LogWarning("Пропуск пустого имени файла");
                continue;
            }

            try
            {
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
                _logger.LogInformation("URL успешно получен для файла {ObjectName}", objectName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении URL для файла {ObjectName}", objectName);
                throw;
            }
        }

        _logger.LogInformation("Получено {Count} URL для дополнительных файлов", urls.Count);
        return urls;
    }
    
    public async Task<byte[]> CreateZipFromFilesAsync(List<string> objectNames, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Начало создания ZIP-архива из {Count} файлов", objectNames?.Count ?? 0);
        
        if (objectNames == null || !objectNames.Any())
        {
            _logger.LogWarning("Попытка создать ZIP-архив из пустого списка файлов");
            return Array.Empty<byte>();
        }

        try
        {
            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                using var httpClient = new HttpClient();
                foreach (var objectName in objectNames)
                {
                    if (string.IsNullOrEmpty(objectName))
                    {
                        _logger.LogWarning("Пропуск пустого имени файла при создании ZIP-архива");
                        continue;
                    }

                    _logger.LogInformation("Добавление файла {ObjectName} в ZIP-архив", objectName);
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

            var zipBytes = memoryStream.ToArray();
            _logger.LogInformation("ZIP-архив успешно создан. Размер: {Size} байт", zipBytes.Length);
            return zipBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании ZIP-архива");
            throw;
        }
    }
}
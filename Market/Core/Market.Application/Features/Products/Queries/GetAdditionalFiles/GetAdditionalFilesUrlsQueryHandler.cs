using System.IO.Compression;
using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Products.Queries.GetAdditionalFiles;

public class GetAdditionalFilesUrlsQueryHandler : IRequestHandler<GetAdditionalFilesUrlsQuery, byte[]>
{
    private readonly IProductRepository _productRepository;
    private readonly IMinioService _minioService;
    private readonly ILogger<GetAdditionalFilesUrlsQueryHandler> _logger;

    public GetAdditionalFilesUrlsQueryHandler(
        IProductRepository productRepository, 
        IMinioService minioService,
        ILogger<GetAdditionalFilesUrlsQueryHandler> logger)
    {
        _productRepository = productRepository;
        _minioService = minioService;
        _logger = logger;
    }

    public async Task<byte[]> Handle(GetAdditionalFilesUrlsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение дополнительных файлов для товара {ProductId}", request.ProductId);
        
        var product = await _productRepository.GetProductByIdAsync(request.ProductId);
        if (product == null)
        {
            _logger.LogWarning("Товар {ProductId} не найден", request.ProductId);
            throw new KeyNotFoundException($"Продукт с ID {request.ProductId} не найден.");
        }

        if (product.AdditionalFilePaths == null || !product.AdditionalFilePaths.Any())
        {
            _logger.LogInformation("У товара {ProductId} нет дополнительных файлов", request.ProductId);
            return Array.Empty<byte>();
        }

        _logger.LogInformation("Создание ZIP-архива с {FileCount} файлами для товара {ProductId}", 
            product.AdditionalFilePaths.Count(), request.ProductId);
            
        var zipBytes = await _minioService.CreateZipFromFilesAsync(product.AdditionalFilePaths, cancellationToken);
        
        _logger.LogInformation("ZIP-архив успешно создан для товара {ProductId}. Размер: {Size} байт", 
            request.ProductId, zipBytes.Length);
            
        return zipBytes;
    }
}
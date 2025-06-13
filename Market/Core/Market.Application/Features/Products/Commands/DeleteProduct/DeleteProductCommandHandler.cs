using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Products.Commands;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IMinioService _minioService;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteProductCommandHandler(
        IProductRepository productRepository,
        IMinioService minioService,
        ILogger<DeleteProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _minioService = minioService;
        _logger = logger;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Начало удаления товара {ProductId}", request.ProductId);
        
        var product = await _productRepository.GetProductByIdAsync(request.ProductId);
        if (product != null)
        {
            if (!string.IsNullOrEmpty(product.CoverImagePath))
            {
                _logger.LogInformation("Удаление обложки товара {ProductId}", request.ProductId);
                await _minioService.DeleteFileAsync(product.CoverImagePath, cancellationToken);
                _logger.LogInformation("Обложка товара {ProductId} успешно удалена", request.ProductId);
            }

            if (product.AdditionalFilePaths != null && product.AdditionalFilePaths.Any())
            {
                _logger.LogInformation("Удаление дополнительных файлов товара {ProductId}. Количество файлов: {FileCount}", 
                    request.ProductId, product.AdditionalFilePaths.Count());
                foreach (var filePath in product.AdditionalFilePaths)
                {
                    await _minioService.DeleteFileAsync(filePath, cancellationToken);
                }
                _logger.LogInformation("Дополнительные файлы товара {ProductId} успешно удалены", request.ProductId);
            }

            await _productRepository.DeleteProductAsync(request.ProductId);
            _logger.LogInformation("Товар {ProductId} успешно удален из базы данных", request.ProductId);
        }
        else
        {
            _logger.LogWarning("Попытка удаления несуществующего товара {ProductId}", request.ProductId);
        }
    }
} 
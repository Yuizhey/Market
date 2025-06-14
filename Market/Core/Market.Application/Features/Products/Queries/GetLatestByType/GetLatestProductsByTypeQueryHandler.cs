using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Products.Queries.GetLatestByType;

public class GetLatestProductsByTypeQueryHandler : IRequestHandler<GetLatestProductsByTypeQuery, IEnumerable<GetLatestProductsByTypeDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMinioService _minioService;
    private readonly ILogger<GetLatestProductsByTypeQueryHandler> _logger;

    public GetLatestProductsByTypeQueryHandler(
        IProductRepository productRepository,
        IMinioService minioService,
        ILogger<GetLatestProductsByTypeQueryHandler> logger)
    {
        _productRepository = productRepository;
        _minioService = minioService;
        _logger = logger;
    }

    public async Task<IEnumerable<GetLatestProductsByTypeDto>> Handle(GetLatestProductsByTypeQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение последних {Count} товаров типа {ProductType}", request.Count, request.Type);
        
        var products = await _productRepository.GetLatestByTypeAsync(request.Type, request.Count);
        _logger.LogInformation("Получено {Count} товаров типа {ProductType}", products.Count(), request.Type);
        
        var result = new List<GetLatestProductsByTypeDto>();
        foreach (var product in products)
        {
            string? imageUrl = null;
            if (!string.IsNullOrEmpty(product.CoverImagePath))
            {
                try
                {
                    _logger.LogInformation("Получение URL обложки для товара {ProductId}", product.Id);
                    imageUrl = await _minioService.GetCoverImageUrlAsync(product.CoverImagePath, cancellationToken);
                    _logger.LogInformation("URL обложки успешно получен для товара {ProductId}", product.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при получении URL обложки для товара {ProductId}", product.Id);
                    imageUrl = "assets/img/520x400.png";
                }
            }

            result.Add(new GetLatestProductsByTypeDto
            {
                Id = product.Id,
                Title = product.Title,
                Subtitle = product.Subtitle,
                Price = product.Price,
                CoverImagePath = imageUrl ?? "/assets/img/520x400.png",
                AuthorName = product.Author?.FirstName + " " + product.Author?.LastName,
                ProductType = product.ProductType
            });
        }

        return result;
    }
} 
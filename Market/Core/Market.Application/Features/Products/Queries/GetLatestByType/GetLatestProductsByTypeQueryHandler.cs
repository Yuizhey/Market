using Market.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Products.Queries.GetLatestByType;

public class GetLatestProductsByTypeQueryHandler : IRequestHandler<GetLatestProductsByTypeQuery, IEnumerable<GetLatestProductsByTypeDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetLatestProductsByTypeQueryHandler> _logger;

    public GetLatestProductsByTypeQueryHandler(
        IProductRepository productRepository,
        ILogger<GetLatestProductsByTypeQueryHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<GetLatestProductsByTypeDto>> Handle(GetLatestProductsByTypeQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение последних {Count} товаров типа {ProductType}", request.Count, request.Type);
        
        var products = await _productRepository.GetLatestByTypeAsync(request.Type, request.Count);
        _logger.LogInformation("Получено {Count} товаров типа {ProductType}", products.Count(), request.Type);
        
        var result = products.Select(p => new GetLatestProductsByTypeDto
        {
            Id = p.Id,
            Title = p.Title,
            Subtitle = p.Subtitle,
            Price = p.Price,
            CoverImagePath = p.CoverImagePath,
            AuthorName = p.Author?.FirstName + " " + p.Author?.LastName,
            ProductType = p.ProductType
        });

        return result;
    }
} 
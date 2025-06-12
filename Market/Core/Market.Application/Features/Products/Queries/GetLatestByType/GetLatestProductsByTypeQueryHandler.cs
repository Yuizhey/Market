using Market.Application.Interfaces.Repositories;
using MediatR;

namespace Market.Application.Features.Products.Queries.GetLatestByType;

public class GetLatestProductsByTypeQueryHandler : IRequestHandler<GetLatestProductsByTypeQuery, IEnumerable<GetLatestProductsByTypeDto>>
{
    private readonly IProductRepository _productRepository;

    public GetLatestProductsByTypeQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<GetLatestProductsByTypeDto>> Handle(GetLatestProductsByTypeQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetLatestByTypeAsync(request.Type, request.Count);
        
        return products.Select(p => new GetLatestProductsByTypeDto
        {
            Id = p.Id,
            Title = p.Title,
            Subtitle = p.Subtitle,
            Price = p.Price,
            CoverImagePath = p.CoverImagePath,
            AuthorName = p.Author?.FirstName + " " + p.Author?.LastName,
            ProductType = p.ProductType
        });
    }
} 
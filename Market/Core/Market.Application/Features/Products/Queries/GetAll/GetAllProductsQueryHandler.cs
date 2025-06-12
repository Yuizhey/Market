using Market.Application.Interfaces.Repositories;
using MediatR;

namespace Market.Application.Features.Products.Queries.GetAll;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<GetAllProductsDto>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<GetAllProductsDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync();
        
        return products.Select(p => new GetAllProductsDto
        {
            Id = p.Id,
            Title = p.Title,
            Subtitle = p.Subtitle,
            Price = p.Price,
            AuthorName = p.Author?.FirstName + " " + p.Author?.LastName,
            CreatedDate = p.CreatedDate ?? DateTime.UtcNow,
            ProductType = p.ProductType.ToString()
        });
    }
} 
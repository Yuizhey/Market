using Market.Application.Interfaces.Repositories;
using MediatR;

namespace Market.Application.Features.Products.Queries.GetByProductId;

public class GetByProductIdQueryHandler : IRequestHandler<GetByProductIdQuery, GetByProductIdDto>
{
    private readonly IProductRepository _productRepository;

    public GetByProductIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<GetByProductIdDto> Handle(GetByProductIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(request.id);
        return new GetByProductIdDto()
        {
            Title = product.Title,
            Price = product.Price,
            Text = product.Text,
            ImageURL = product.CoverImagePath
        };
    }
}
using Market.Application.Interfaces.Repositories;
using MediatR;

namespace Market.Application.Features.Products.Queries.GetByPageNumber;

public class GetByPageNumberQueryHandler : IRequestHandler<GetByPageNumberQuery, IEnumerable<GetByPageNumberDto>>
{
    private readonly IProductRepository _productRepository;

    public GetByPageNumberQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<GetByPageNumberDto>> Handle(GetByPageNumberQuery request, CancellationToken cancellationToken)
    {
        var products =  await _productRepository.GetProductsByPage(request.page, request.pageSize);
        return products.Select(p => new GetByPageNumberDto
        {
            Id = p.Id,
            Title = p.Title,
            Text = p.Text,
            Price = p.Price
        });
    }
}
using Market.Application.Interfaces.Repositories;
using MediatR;

namespace Market.Application.Features.Products.Queries.GetByPageNumber;

public class GetByPageNumberQueryHandler : IRequestHandler<GetByPageNumberQuery, GetByPageNumberREsult>
{
    private readonly IProductRepository _productRepository;

    public GetByPageNumberQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<GetByPageNumberREsult> Handle(GetByPageNumberQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await _productRepository.GetTotalProductCountAsync();
        var products = await _productRepository.GetProductsByPage(request.Page, request.PageSize);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new GetByPageNumberREsult
        {
            Products = products.Select(p => new GetByPageNumberDto
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price
            }),
            TotalPages = totalPages,
            CurrentPage = request.Page
        };
    }
}

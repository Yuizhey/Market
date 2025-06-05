using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using MediatR;

namespace Market.Application.Features.Products.Queries.GetByPageNumber;

public class GetByPageNumberQueryHandler : IRequestHandler<GetByPageNumberQuery, GetByPageNumberREsult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMinioService _minioService;

    public GetByPageNumberQueryHandler(
        IProductRepository productRepository,
        IMinioService minioService)
    {
        _productRepository = productRepository;
        _minioService = minioService;
    }

    public async Task<GetByPageNumberREsult> Handle(GetByPageNumberQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProductsByPage(request.Page, request.PageSize);
        var totalCount = await _productRepository.GetTotalProductCountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        var productDtos = new List<GetByPageNumberDto>();
        foreach (var product in products)
        {
            string? imageUrl = null;
            if (!string.IsNullOrEmpty(product.CoverImagePath))
            {
                try
                {
                    imageUrl = await _minioService.GetCoverImageUrlAsync(product.CoverImagePath, cancellationToken);
                }
                catch (Exception)
                {
                    imageUrl = "assets/img/520x400.png"; 
                }
            }

            productDtos.Add(new GetByPageNumberDto
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,
                ImageURL = imageUrl ?? "assets/img/520x400.png"
            });
        }

        return new GetByPageNumberREsult
        {
            Products = productDtos,
            TotalPages = totalPages,
            CurrentPage = request.Page
        };
    }
}

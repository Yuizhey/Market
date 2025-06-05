using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using MediatR;

namespace Market.Application.Features.Products.Queries.GetByProductId;

public class GetByProductIdQueryHandler : IRequestHandler<GetByProductIdQuery, GetByProductIdDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMinioService _minioService;

    public GetByProductIdQueryHandler(IProductRepository productRepository,
        IMinioService minioService)
    {
        _productRepository = productRepository;
        _minioService = minioService;
    }
    public async Task<GetByProductIdDto> Handle(GetByProductIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(request.id);
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
        return new GetByProductIdDto()
        {
            Title = product.Title,
            Price = product.Price,
            Text = product.Text,
            ImageURL = imageUrl ?? "assets/img/520x400.png"
        };
    }
}
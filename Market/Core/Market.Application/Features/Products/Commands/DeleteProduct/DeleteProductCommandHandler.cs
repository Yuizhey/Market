using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using MediatR;

namespace Market.Application.Features.Products.Commands;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IMinioService _minioService;

    public DeleteProductCommandHandler(
        IProductRepository productRepository,
        IMinioService minioService)
    {
        _productRepository = productRepository;
        _minioService = minioService;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(request.ProductId);
        if (product != null)
        {
            if (!string.IsNullOrEmpty(product.CoverImagePath))
            {
                await _minioService.DeleteFileAsync(product.CoverImagePath, cancellationToken);
            }
            await _productRepository.DeleteProductAsync(request.ProductId);
        }
    }
} 
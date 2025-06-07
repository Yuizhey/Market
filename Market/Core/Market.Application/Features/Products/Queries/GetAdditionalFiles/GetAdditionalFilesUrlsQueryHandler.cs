using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using MediatR;

namespace Market.Application.Features.Products.Queries.GetAdditionalFiles;

public class GetAdditionalFilesUrlsQueryHandler : IRequestHandler<GetAdditionalFilesUrlsQuery, List<string>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMinioService _minioService;

    public GetAdditionalFilesUrlsQueryHandler(IProductRepository productRepository, IMinioService minioService)
    {
        _productRepository = productRepository;
        _minioService = minioService;
    }

    public async Task<List<string>> Handle(GetAdditionalFilesUrlsQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(request.ProductId);
        if (product == null)
        {
            throw new KeyNotFoundException($"Продукт с ID {request.ProductId} не найден.");
        }

        return await _minioService.GetAdditionalFilesUrlsAsync(product.AdditionalFilePaths, cancellationToken);
    }
}
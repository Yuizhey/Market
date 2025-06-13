using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Products.Queries.GetByProductId;

public class GetByProductIdQueryHandler : IRequestHandler<GetByProductIdQuery, GetByProductIdDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMinioService _minioService;
    private readonly ILikeRepository _likeRepository;
    private readonly ILogger<GetByProductIdQueryHandler> _logger;

    public GetByProductIdQueryHandler(
        IProductRepository productRepository,
        IMinioService minioService,
        ILikeRepository likeRepository,
        ILogger<GetByProductIdQueryHandler> logger)
    {
        _productRepository = productRepository;
        _minioService = minioService;
        _likeRepository = likeRepository;
        _logger = logger;
    }

    public async Task<GetByProductIdDto> Handle(GetByProductIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Получение информации о товаре {ProductId}", request.id);
        
        var product = await _productRepository.GetProductByIdAsync(request.id);
        if (product == null)
        {
            _logger.LogWarning("Товар {ProductId} не найден", request.id);
            return null;
        }

        string? imageUrl = null;
        if (!string.IsNullOrEmpty(product.CoverImagePath))
        {
            try
            {
                _logger.LogInformation("Получение URL обложки для товара {ProductId}", request.id);
                imageUrl = await _minioService.GetCoverImageUrlAsync(product.CoverImagePath, cancellationToken);
                _logger.LogInformation("URL обложки успешно получен для товара {ProductId}", request.id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении URL обложки для товара {ProductId}", request.id);
                imageUrl = "assets/img/520x400.png"; 
            }
        }

        var likesCount = await _likeRepository.GetLikesCountByProductIdAsync(product.Id);
        _logger.LogInformation("Получено количество лайков для товара {ProductId}: {LikesCount}", request.id, likesCount);

        var dto = new GetByProductIdDto()
        {
            Id = product.Id,
            Title = product.Title,
            Price = product.Price,
            Text = product.Text,
            ImageURL = imageUrl ?? "assets/img/520x400.png",
            LikesCount = likesCount
        };

        _logger.LogInformation("Информация о товаре {ProductId} успешно получена", request.id);
        return dto;
    }
}
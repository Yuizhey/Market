using Market.Application.Interfaces.Repositories;
using Market.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Market.Domain.Enums;

namespace Market.Application.Features.Products.Queries.GetByProductId;

public class GetByProductIdQueryHandler : IRequestHandler<GetByProductIdQuery, GetByProductIdDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMinioService _minioService;
    private readonly ILikeRepository _likeRepository;
    private readonly ILogger<GetByProductIdQueryHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserDescriptionRepository _userDescriptionRepository;
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;

    public GetByProductIdQueryHandler(
        IProductRepository productRepository,
        IMinioService minioService,
        ILikeRepository likeRepository,
        ILogger<GetByProductIdQueryHandler> logger,
        IHttpContextAccessor httpContextAccessor,
        IUserDescriptionRepository userDescriptionRepository,
        IAuthorUserDescriptionRepository authorUserDescriptionRepository)
    {
        _productRepository = productRepository;
        _minioService = minioService;
        _likeRepository = likeRepository;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _userDescriptionRepository = userDescriptionRepository;
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
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

        bool isLiked = false;
        var identityUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

        if (!string.IsNullOrEmpty(identityUserId) && !string.IsNullOrEmpty(userRole))
        {
            try
            {
                Guid userId;
                if (userRole == UserRoles.CLientUser.ToString())
                {
                    var userDescriptionId = await _userDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId));
                    if (userDescriptionId != null)
                    {
                        userId = userDescriptionId;
                        isLiked = await _likeRepository.IsProductLikedByUserAsync(product.Id, userId);
                    }
                }
                else if (userRole == UserRoles.AuthorUser.ToString())
                {
                    var authorUserDescriptionId = await _authorUserDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId));
                    if (authorUserDescriptionId != null)
                    {
                        userId = authorUserDescriptionId;
                        isLiked = await _likeRepository.IsProductLikedByUserAsync(product.Id, userId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при проверке лайка пользователя для товара {ProductId}", request.id);
            }
        }

        var dto = new GetByProductIdDto()
        {
            Id = product.Id,
            Title = product.Title,
            Subtitle = product.Subtitle,
            Price = product.Price,
            Text = product.Text,
            ImageURL = imageUrl ?? "assets/img/520x400.png",
            LikesCount = likesCount,
            IsLiked = isLiked
        };

        _logger.LogInformation("Информация о товаре {ProductId} успешно получена", request.id);
        return dto;
    }
}
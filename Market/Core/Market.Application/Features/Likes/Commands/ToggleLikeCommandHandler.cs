using System.Security.Claims;
using Market.Application.Features.Likes.Commands;
using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class ToggleLikeCommandHandler : IRequestHandler<ToggleLikeCommand>
{
    private readonly ILikeRepository _likeRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserDescriptionRepository _userDescriptionRepository;
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;
    private readonly ILogger<ToggleLikeCommandHandler> _logger;

    public ToggleLikeCommandHandler(
        ILikeRepository likeRepository,
        IHttpContextAccessor httpContextAccessor,
        IUserDescriptionRepository userDescriptionRepository,
        IAuthorUserDescriptionRepository authorUserDescriptionRepository,
        ILogger<ToggleLikeCommandHandler> logger)
    {
        _likeRepository = likeRepository;
        _httpContextAccessor = httpContextAccessor;
        _userDescriptionRepository = userDescriptionRepository;
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
        _logger = logger;
    }

    public async Task Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
    {
        var identityUserId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        _logger.LogInformation("Попытка переключения лайка для товара {ProductId} пользователем {UserId} с ролью {Role}", 
            request.ProductId, identityUserId, userRole);

        Guid userId;
        if (userRole == UserRoles.CLientUser.ToString())
        {
            var businessId = await _userDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId!));
            if (businessId == null)
            {
                _logger.LogError("Не найден профиль пользователя для {UserId}", identityUserId);
                throw new InvalidOperationException("User description not found");
            }
            userId = businessId;
        }
        else if (userRole == UserRoles.AuthorUser.ToString())
        {
            var businessId = await _authorUserDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId!));
            if (businessId == null)
            {
                _logger.LogError("Не найден профиль автора для {UserId}", identityUserId);
                throw new InvalidOperationException("Author user description not found");
            }
            userId = businessId;
        }
        else
        {
            _logger.LogError("Некорректная роль пользователя {Role} для {UserId}", userRole, identityUserId);
            throw new UnauthorizedAccessException();
        }

        var existingLike = await _likeRepository.GetByUserAndProductAsync(userId, request.ProductId);

        if (existingLike is not null)
        {
            _logger.LogInformation("Удаление лайка для товара {ProductId} пользователем {UserId}", request.ProductId, userId);
            await _likeRepository.DeleteAsync(existingLike);
        }
        else
        {
            _logger.LogInformation("Добавление лайка для товара {ProductId} пользователем {UserId}", request.ProductId, userId);
            var newLike = new Like
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProductId = request.ProductId
            };
            await _likeRepository.AddAsync(newLike);
        }
    }
}

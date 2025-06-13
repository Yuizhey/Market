using System.Security.Claims;
using Market.Application.Interfaces.Repositories;
using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Carts.Commands.AddItemToCart;

public sealed class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserDescriptionRepository _userDescriptionRepository;
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AddItemToCartCommandHandler> _logger;

    public AddItemToCartCommandHandler(
        ICartRepository cartRepository,
        IUserDescriptionRepository userDescriptionRepository,
        IAuthorUserDescriptionRepository authorUserDescriptionRepository,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AddItemToCartCommandHandler> logger)
    {
        _cartRepository = cartRepository;
        _userDescriptionRepository = userDescriptionRepository;
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var identityUserId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        _logger.LogInformation("Попытка добавления товара {ItemId} в корзину пользователя {UserId} с ролью {Role}", 
            request.itemId, identityUserId, userRole);

        Guid userId;
        if (userRole == UserRoles.CLientUser.ToString())
        {
            var userDescriptionId = await _userDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId!));
            if (userDescriptionId == null)
            {
                _logger.LogError("Не найден профиль пользователя для {UserId}", identityUserId);
                throw new InvalidOperationException("User description not found");
            }
            userId = userDescriptionId;
        }
        else if (userRole == UserRoles.AuthorUser.ToString())
        {
            var authorUserDescriptionId = await _authorUserDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId!));
            if (authorUserDescriptionId == null)
            {
                _logger.LogError("Не найден профиль автора для {UserId}", identityUserId);
                throw new InvalidOperationException("Author user description not found");
            }
            userId = authorUserDescriptionId;
        }
        else
        {
            _logger.LogError("Некорректная роль пользователя {Role} для {UserId}", userRole, identityUserId);
            throw new InvalidOperationException("Invalid user role");
        }

        await _cartRepository.AddProductToCartAsync(userId, request.itemId);
        _logger.LogInformation("Товар {ItemId} успешно добавлен в корзину пользователя {UserId}", request.itemId, userId);
    }
}

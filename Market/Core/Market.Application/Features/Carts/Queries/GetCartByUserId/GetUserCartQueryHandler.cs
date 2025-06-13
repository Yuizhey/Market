using System.Security.Claims;
using Market.Application.Interfaces.Repositories;
using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Market.Application.Features.Carts.Queries.GetCartByUserId;

public class GetUserCartQueryHandler : IRequestHandler<GetUserCartQuery, GetUserCartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserDescriptionRepository _userDescriptionRepository;
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<GetUserCartQueryHandler> _logger;

    public GetUserCartQueryHandler(
        ICartRepository cartRepository,
        IUserDescriptionRepository userDescriptionRepository,
        IAuthorUserDescriptionRepository authorUserDescriptionRepository,
        IHttpContextAccessor httpContextAccessor,
        ILogger<GetUserCartQueryHandler> logger)
    {
        _cartRepository = cartRepository;
        _userDescriptionRepository = userDescriptionRepository;
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<GetUserCartDto> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
    {
        var identityUserId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        _logger.LogInformation("Получение корзины пользователя {UserId} с ролью {Role}", identityUserId, userRole);
        
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

        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null)
        {
            _logger.LogInformation("Корзина не найдена для пользователя {UserId}", userId);
            return new GetUserCartDto();
        }

        _logger.LogInformation("Получена корзина пользователя {UserId} с {ItemCount} товарами", userId, cart.Items.Count);

        var dto = new GetUserCartDto
        {
            CartId = cart.Id,
            Items = cart.Items.Select(i => new CartItemDto
            {
                ProductId = i.Product.Id,
                Title = i.Product.Title,
                Price = i.Product.Price
            }).ToList()
        };

        return dto;
    }
}
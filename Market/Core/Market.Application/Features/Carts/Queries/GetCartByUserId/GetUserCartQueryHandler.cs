using System.Security.Claims;
using Market.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Features.Carts.Queries.GetCartByUserId;

public class GetUserCartHandler : IRequestHandler<GetUserCartQuery, GetUserCartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetUserCartHandler(ICartRepository cartRepository, IHttpContextAccessor httpContextAccessor)
    {
        _cartRepository = cartRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetUserCartDto> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
        {
            throw new UnauthorizedAccessException("Пользователь не аутентифицирован.");
        }

        var cart = await _cartRepository.GetByUserIdAsync(parsedUserId);

        if (cart == null)
            return new GetUserCartDto();

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
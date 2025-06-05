using System.Security.Claims;
using Market.Application.Interfaces.Repositories;
using Market.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Features.Carts.Queries.GetCartByUserId;

public class GetUserCartHandler : IRequestHandler<GetUserCartQuery, GetUserCartDto>
{
    private readonly ICartRepository _cartRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserDescriptionRepository _userDescriptionRepository;
    private readonly IAuthorUserDescriptionRepository _authorUserDescriptionRepository;

    public GetUserCartHandler(
        ICartRepository cartRepository, 
        IHttpContextAccessor httpContextAccessor,
        IUserDescriptionRepository userDescriptionRepository,
        IAuthorUserDescriptionRepository authorUserDescriptionRepository)
    {
        _cartRepository = cartRepository;
        _httpContextAccessor = httpContextAccessor;
        _userDescriptionRepository = userDescriptionRepository;
        _authorUserDescriptionRepository = authorUserDescriptionRepository;
    }

    public async Task<GetUserCartDto> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
    {
        var identityUserId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        
        Guid userId;
        if (userRole == UserRoles.CLientUser.ToString())
        {
            var userDescriptionId = await _userDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId!));
            if (userDescriptionId == null)
                throw new InvalidOperationException("User description not found");
            userId = userDescriptionId;
        }
        else if (userRole == UserRoles.AuthorUser.ToString())
        {
            var authorUserDescriptionId = await _authorUserDescriptionRepository.GetBusinessIdByIdentityUserIdAsync(Guid.Parse(identityUserId!));
            if (authorUserDescriptionId == null)
                throw new InvalidOperationException("Author user description not found");
            userId = authorUserDescriptionId;
        }
        else
        {
            throw new InvalidOperationException("Invalid user role");
        }
        var cart = await _cartRepository.GetByUserIdAsync(userId);
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
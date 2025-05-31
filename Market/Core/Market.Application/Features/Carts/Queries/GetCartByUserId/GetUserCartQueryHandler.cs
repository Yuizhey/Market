using Market.Application.Interfaces.Repositories;
using MediatR;

namespace Market.Application.Features.Carts.Queries.GetCartByUserId;

public class GetUserCartHandler : IRequestHandler<GetUserCartQuery, GetUserCartDto>
{
    private readonly ICartRepository _cartRepository;

    public GetUserCartHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<GetUserCartDto> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdAsync(request.UserId);

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
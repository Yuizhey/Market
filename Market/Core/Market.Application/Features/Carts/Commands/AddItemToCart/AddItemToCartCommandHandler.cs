using Market.Application.Interfaces.Repositories;
using MediatR;

namespace Market.Application.Features.Carts.Commands.AddItemToCart;

public sealed class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand>
{
    private readonly ICartRepository _cartRepository;

    public AddItemToCartCommandHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        await _cartRepository.AddProductToCartAsync(request.userId, request.itemId);
    }
}

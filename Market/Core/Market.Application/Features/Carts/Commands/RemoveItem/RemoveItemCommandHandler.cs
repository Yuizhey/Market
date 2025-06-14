using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using MediatR;

namespace Market.Application.Features.Carts.Commands.RemoveItem;

public class RemoveItemCommandHandler : IRequestHandler<RemoveItemCommand, Unit>
{
    private readonly ICartRepository _cartRepository;
    private readonly IGenericRepository<Cart> _genericRepository;

    public RemoveItemCommandHandler(
        ICartRepository cartRepository,
        IGenericRepository<Cart> genericRepository)
    {
        _cartRepository = cartRepository;
        _genericRepository = genericRepository;
    }

    public async Task<Unit> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.CartId);
        if (cart == null)
        {
            throw new Exception("Корзина не найдена");
        }

        var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (cartItem == null)
        {
            throw new Exception("Товар не найден в корзине");
        }

        cart.Items.Remove(cartItem);
        await _genericRepository.UpdateAsync(cart);

        return Unit.Value;
    }
} 
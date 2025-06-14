using Market.Application.Interfaces.Repositories;
using MediatR;

namespace Market.Application.Features.Carts.Commands.RemoveItem;

public class RemoveItemCommandHandler : IRequestHandler<RemoveItemCommand, Unit>
{
    private readonly ICartRepository _cartRepository;

    public RemoveItemCommandHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Unit> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        await _cartRepository.RemoveProductFromCartAsync(request.CartId, request.ProductId);
        return Unit.Value;
    }
} 
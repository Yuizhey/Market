using MediatR;

namespace Market.Application.Features.Carts.Commands.RemoveItem;

public class RemoveItemCommand : IRequest<Unit>
{
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
} 
using MediatR;

namespace Market.Application.Features.Carts.Commands.AddItemToCart;

public record AddItemToCartCommand(Guid userId, Guid itemId) : IRequest;

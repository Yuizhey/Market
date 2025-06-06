using MediatR;

namespace Market.Application.Features.Carts.Commands.CheckoutCart;

public record CheckoutCartCommand(Guid CartId, string Email) : IRequest; 
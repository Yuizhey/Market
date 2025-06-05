using MediatR;

namespace Market.Application.Features.Carts.Queries.GetCartByUserId;

public record GetUserCartQuery : IRequest<GetUserCartDto>;
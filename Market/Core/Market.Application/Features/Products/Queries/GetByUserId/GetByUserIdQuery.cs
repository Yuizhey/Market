using MediatR;

namespace Market.Application.Features.Products.Queries.GetByUserId;

public record GetByUserIdQuery : IRequest<IEnumerable<GetByUserIdDto>>;

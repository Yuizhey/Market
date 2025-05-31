using Market.Application.Features.Products.Queries.GetByPageNumber;
using MediatR;

namespace Market.Application.Features.Products.Queries.GetByProductId;

public record GetByProductIdQuery(Guid id): IRequest<GetByProductIdDto>;
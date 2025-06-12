using MediatR;

namespace Market.Application.Features.Products.Queries.GetAll;

public record GetAllProductsQuery : IRequest<IEnumerable<GetAllProductsDto>>; 
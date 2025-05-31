using MediatR;

namespace Market.Application.Features.Products.Queries.GetByPageNumber;

public record GetByPageNumberQuery(int page, int pageSize) : IRequest<IEnumerable<GetByPageNumberDto>>;

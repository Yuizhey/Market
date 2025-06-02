using MediatR;

namespace Market.Application.Features.Products.Queries.GetByPageNumber;

public record GetByPageNumberQuery(int Page, int PageSize) : IRequest<GetByPageNumberREsult>;


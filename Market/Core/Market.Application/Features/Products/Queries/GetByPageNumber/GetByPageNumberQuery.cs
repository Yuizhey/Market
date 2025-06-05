using Market.Domain.Enums;
using MediatR;

namespace Market.Application.Features.Products.Queries.GetByPageNumber;

public record GetByPageNumberQuery(int Page, int PageSize, IEnumerable<ProductType>? Types = null) : IRequest<GetByPageNumberREsult>;


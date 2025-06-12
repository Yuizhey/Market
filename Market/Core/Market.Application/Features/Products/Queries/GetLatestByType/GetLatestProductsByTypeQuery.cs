using Market.Domain.Enums;
using MediatR;

namespace Market.Application.Features.Products.Queries.GetLatestByType;

public record GetLatestProductsByTypeQuery(ProductType Type, int Count = 5) : IRequest<IEnumerable<GetLatestProductsByTypeDto>>; 
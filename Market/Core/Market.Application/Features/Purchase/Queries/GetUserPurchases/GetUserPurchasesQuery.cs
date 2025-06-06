using MediatR;

namespace Market.Application.Features.Purchase.Queries.GetUserPurchases;

public record GetUserPurchasesQuery : IRequest<IEnumerable<PurchaseDto>>; 
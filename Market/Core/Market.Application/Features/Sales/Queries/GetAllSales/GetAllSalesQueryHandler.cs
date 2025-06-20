using Market.Application.Interfaces.Repositories;
using MediatR;

namespace Market.Application.Features.Sales.Queries.GetAllSales;

public class GetAllSalesQueryHandler : IRequestHandler<GetAllSalesQuery, IEnumerable<GetAllSalesDto>>
{
    private readonly IPurchaseRepository _purchaseRepository;

    public GetAllSalesQueryHandler(IPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    public async Task<IEnumerable<GetAllSalesDto>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
    {
        // Здесь должен быть метод получения всех продаж, добавим его позже
        var purchases = await _purchaseRepository.GetAllAsync();
        return purchases.Select(p => new GetAllSalesDto
        {
            Id = p.Id,
            ProductTitle = p.Product.Title,
            BuyerId = p.BuyerId.ToString(),
            SellerName = p.Seller?.FirstName + " " + p.Seller?.LastName,
            Price = p.Price,
            PurchaseDate = p.PurchaseDate,
            Quantity = p.Quantity
        });
    }
} 
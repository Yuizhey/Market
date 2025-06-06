using Market.Domain.Entities;

namespace Market.Application.Interfaces.Repositories;

public interface IPurchaseRepository 
{
    Task<Purchase> AddPurchaseAsync(Purchase purchase);
    Task<IEnumerable<Purchase>> GetPurchasesByBuyerIdAsync(Guid buyerId);
    Task<IEnumerable<Purchase>> GetPurchasesBySellerIdAsync(Guid sellerId);
} 
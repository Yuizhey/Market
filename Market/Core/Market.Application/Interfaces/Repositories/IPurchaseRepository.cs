using Market.Domain.Entities;

namespace Market.Application.Interfaces.Repositories;

public interface IPurchaseRepository 
{
    Task<Purchase> AddPurchaseAsync(Purchase purchase);
} 
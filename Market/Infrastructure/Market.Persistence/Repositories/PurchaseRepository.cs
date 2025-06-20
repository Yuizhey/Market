using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Market.Persistence.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly IGenericRepository<Purchase> _repository;

    public PurchaseRepository(IGenericRepository<Purchase> repository)
    {
        _repository = repository;
    }

    public async Task<Purchase> AddPurchaseAsync(Purchase purchase)
    {
        return await _repository.AddAsync(purchase);
    }

    public async Task<IEnumerable<Purchase>> GetPurchasesByBuyerIdAsync(Guid buyerId)
    {
        return await _repository.Entities
            .Where(p => p.BuyerId == buyerId)
            .Include(p => p.Product)
            .ToListAsync();
    }

    public async Task<IEnumerable<Purchase>> GetPurchasesBySellerIdAsync(Guid sellerId)
    {
        return await _repository.Entities
            .Where(p => p.SellerId == sellerId)
            .Include(p => p.Product)
            .ToListAsync();
    }

    public async Task<IEnumerable<Purchase>> GetAllAsync()
    {
        return await _repository.Entities
            .Include(p => p.Product)
            .Include(p => p.Seller)
            .ToListAsync();
    }
} 
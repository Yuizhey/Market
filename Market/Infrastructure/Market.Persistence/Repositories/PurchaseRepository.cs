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
} 
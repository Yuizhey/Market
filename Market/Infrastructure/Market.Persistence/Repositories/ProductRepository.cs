using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Market.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IGenericRepository<Product> _repository;

    public ProductRepository(IGenericRepository<Product> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Product>> GetProductsByPage(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        return await _repository.Entities
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsTracking()
            .ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(Guid id)
    {
        return await _repository.Entities.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddProductAsync(Product product)
    {
        await _repository.AddAsync(product);
    }
    
    public async Task<int> GetTotalProductCountAsync()
    {
        return await _repository.Entities.CountAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByUserId(Guid userId)
    {
        return await _repository.Entities.Where(e => e.AuthorUserId == userId).ToListAsync();
    }
}
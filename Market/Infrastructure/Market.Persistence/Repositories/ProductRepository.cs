using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using Market.Domain.Enums;
using Market.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Market.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetProductsByPage(int page, int pageSize)
    {
        return await _context.Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByTypes(IEnumerable<ProductType> types, int page, int pageSize)
    {
        return await _context.Products
            .Where(p => types.Contains(p.ProductType))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalProductCountAsync()
    {
        return await _context.Products.CountAsync();
    }

    public async Task<int> GetTotalProductCountByTypesAsync(IEnumerable<ProductType> types)
    {
        return await _context.Products
            .Where(p => types.Contains(p.ProductType))
            .CountAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task AddProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Product>> GetFilteredProductsAsync(
        int page, 
        int pageSize, 
        IEnumerable<ProductType>? types = null,
        decimal? minPrice = null,
        decimal? maxPrice = null)
    {
        var query = _context.Products.AsQueryable();

        if (types != null && types.Any())
        {
            query = query.Where(p => types.Contains(p.ProductType));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetFilteredProductsCountAsync(
        IEnumerable<ProductType>? types = null,
        decimal? minPrice = null,
        decimal? maxPrice = null)
    {
        var query = _context.Products.AsQueryable();

        if (types != null && types.Any())
        {
            query = query.Where(p => types.Contains(p.ProductType));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        return await query.CountAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByUserId(Guid userId)
    {
        return await _context.Products
            .Where(e => e.AuthorUserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
    }
}
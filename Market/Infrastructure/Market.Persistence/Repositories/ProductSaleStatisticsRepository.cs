using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using Market.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Market.Persistence.Repositories;

public class ProductSaleStatisticsRepository : IProductSaleStatisticsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductSaleStatisticsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProductSaleStatistics?> GetByProductIdAsync(Guid productId)
    {
        return await _dbContext.ProductSaleStatistics
            .FirstOrDefaultAsync(s => s.ProductId == productId);
    }

    public async Task UpdateProductStatisticsAsync(Guid productId, decimal salePrice)
    {
        var statistics = await GetByProductIdAsync(productId);
        
        if (statistics == null)
        {
            statistics = new ProductSaleStatistics
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                TotalSalesCount = 1,
                TotalRevenue = salePrice
            };
            await _dbContext.ProductSaleStatistics.AddAsync(statistics);
        }
        else
        {
            statistics.TotalSalesCount++;
            statistics.TotalRevenue += salePrice;
        }

        await _dbContext.SaveChangesAsync();
    }
} 
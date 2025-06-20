using Market.Domain.Entities;

namespace Market.Application.Interfaces.Repositories;

public interface IProductSaleStatisticsRepository
{
    Task UpdateProductStatisticsAsync(Guid productId, decimal salePrice);
    Task<ProductSaleStatistics?> GetByProductIdAsync(Guid productId);
    Task<IEnumerable<ProductSaleStatistics>> GetByProductIdsAsync(IEnumerable<Guid> productIds);
    Task<IEnumerable<ProductSaleStatistics>> GetAllAsync();
} 
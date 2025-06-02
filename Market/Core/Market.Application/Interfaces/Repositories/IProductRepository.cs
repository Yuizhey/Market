using Market.Domain.Entities;

namespace Market.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsByPage(int page, int pageSize);
    Task<Product> GetProductByIdAsync(Guid id);
    Task AddProductAsync(Product product);
    Task<int> GetTotalProductCountAsync();
}
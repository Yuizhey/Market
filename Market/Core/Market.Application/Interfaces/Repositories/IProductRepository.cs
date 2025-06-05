using Market.Domain.Entities;
using Market.Domain.Enums;

namespace Market.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsByPage(int page, int pageSize);
    Task<Product> GetProductByIdAsync(Guid id);
    Task AddProductAsync(Product product);
    Task DeleteProductAsync(Guid id);
    Task<int> GetTotalProductCountAsync();
    Task<IEnumerable<Product>> GetProductsByUserId(Guid userId);
    Task<IEnumerable<Product>> GetProductsByTypes(IEnumerable<ProductType> types, int page, int pageSize);
    Task<int> GetTotalProductCountByTypesAsync(IEnumerable<ProductType> types);
}
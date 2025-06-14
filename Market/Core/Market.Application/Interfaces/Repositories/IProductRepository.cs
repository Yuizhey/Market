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
    Task<Product?> GetByIdAsync(Guid id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Product>> GetFilteredProductsAsync(
        int page, 
        int pageSize, 
        IEnumerable<ProductType>? types = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? searchTerm = null);
    Task<int> GetFilteredProductsCountAsync(
        IEnumerable<ProductType>? types = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? searchTerm = null);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetLatestByTypeAsync(ProductType type, int count);
}
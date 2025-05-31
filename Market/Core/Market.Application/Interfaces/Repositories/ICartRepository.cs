using Market.Domain.Entities;

namespace Market.Application.Interfaces.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(Guid userId);
    Task AddProductToCartAsync(Guid userId, Guid productId);
}
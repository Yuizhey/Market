using Market.Domain.Entities;

namespace Market.Application.Interfaces.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetByIdAsync(Guid id);
    Task<Cart?> GetByUserIdAsync(Guid userId);
    Task AddProductToCartAsync(Guid userId, Guid productId);
    Task RemoveProductFromCartAsync(Guid cartId, Guid productId);
    Task DeleteCartAsync(Guid cartId);
}
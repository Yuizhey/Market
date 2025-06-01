using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using Market.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Market.Persistence.Repositories;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CartRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Cart?> GetByUserIdAsync(Guid userId)
    {
        return await _dbContext.Carts
            .Include(c => c.Items)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task AddProductToCartAsync(Guid userId, Guid productId)
    {
        var cart = await _dbContext.Carts
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId
            };
            _dbContext.Carts.Add(cart);
            await _dbContext.SaveChangesAsync(); 
        }
        
        var itemExists = await _dbContext.CartItems
            .AnyAsync(ci => ci.CartId == cart.Id && ci.ProductId == productId);

        if (!itemExists)
        {
            _dbContext.CartItems.Add(new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cart.Id,
                ProductId = productId
            });

            await _dbContext.SaveChangesAsync();
        }
    }
}

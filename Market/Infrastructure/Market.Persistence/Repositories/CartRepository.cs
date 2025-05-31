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
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Items = new List<CartItem>()
            };
            _dbContext.Carts.Add(cart);
        }

        if (!cart.Items.Any(i => i.ProductId == productId))
        {
            cart.Items.Add(new CartItem
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                CartId = cart.Id
            });
        }

        await _dbContext.SaveChangesAsync();
    }
}

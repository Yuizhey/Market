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
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task AddProductToCartAsync(Guid userId, Guid productId)
    {
        var cart = await GetByUserIdAsync(userId);
        if (cart == null)
        {
            cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Items = new List<CartItem>()
            };
            await _dbContext.Carts.AddAsync(cart);
        }

        var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (cartItem == null)
        {
            cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cart.Id,
                ProductId = productId
            };
            cart.Items.Add(cartItem);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCartAsync(Guid cartId)
    {
        var cart = await _dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == cartId);

        if (cart != null)
        {
            _dbContext.CartItems.RemoveRange(cart.Items);
            
            _dbContext.Carts.Remove(cart);
            
            await _dbContext.SaveChangesAsync();
        }
    }
    
    public async Task<Cart?> GetByIdAsync(Guid Id)
    {
        return await _dbContext.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.Id == Id);
    }
}

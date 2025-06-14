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
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            // Получаем корзину с блокировкой строки
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
                await _dbContext.Carts.AddAsync(cart);
                await _dbContext.SaveChangesAsync();
            }

            // Проверяем, есть ли уже такой товар в корзине
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem == null)
            {
                var cartItem = new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    ProductId = productId
                };
                
                // Добавляем новый элемент
                await _dbContext.CartItems.AddAsync(cartItem);
                await _dbContext.SaveChangesAsync();
            }

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task RemoveProductFromCartAsync(Guid cartId, Guid productId)
    {
        var cart = await _dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == cartId);

        if (cart == null)
        {
            throw new Exception("Корзина не найдена");
        }

        var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (cartItem == null)
        {
            throw new Exception("Товар не найден в корзине");
        }

        _dbContext.CartItems.Remove(cartItem);
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

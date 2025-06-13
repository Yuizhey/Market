using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using Market.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

public class LikeRepository : ILikeRepository
{
    private readonly IGenericRepository<Like> _genericRepository;

    public LikeRepository(IGenericRepository<Like> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task<Like?> GetByUserAndProductAsync(Guid userId, Guid productId)
    {
        return await _genericRepository.Entities
            .FirstOrDefaultAsync(l => l.UserId == userId && l.ProductId == productId);
    }

    public async Task AddAsync(Like like)
    {
        await _genericRepository.AddAsync(like);
    }

    public async Task DeleteAsync(Like like)
    {
        await _genericRepository.DeleteAsync(like);
    }
    
    public async Task<int> GetLikesCountByProductIdAsync(Guid productId)
    {
        return await _genericRepository.Entities
            .CountAsync(l => l.ProductId == productId);
    }
    
    public async Task<List<Guid>> GetLikedProductIdsByUserIdAsync(Guid userId)
    {
        return await _genericRepository.Entities
            .Where(l => l.UserId == userId)
            .Select(l => l.ProductId)
            .ToListAsync();
    }

    public async Task<bool> IsProductLikedByUserAsync(Guid productId, Guid userId)
    {
        return await _genericRepository.Entities
            .AnyAsync(l => l.ProductId == productId && l.UserId == userId);
    }
}
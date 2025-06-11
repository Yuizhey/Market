using Market.Domain.Entities;

namespace Market.Application.Interfaces.Repositories;

public interface ILikeRepository
{
    Task<Like?> GetByUserAndProductAsync(Guid userId, Guid productId);
    Task AddAsync(Like like);
    Task DeleteAsync(Like like);
    Task<int> GetLikesCountByProductIdAsync(Guid productId);
    Task<List<Guid>> GetLikedProductIdsByUserIdAsync(Guid userId);
}

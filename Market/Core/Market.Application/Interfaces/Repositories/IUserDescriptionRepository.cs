using Market.Domain.Entities;

namespace Market.Application.Interfaces.Repositories;

public interface IUserDescriptionRepository
{
    Task<Guid> GetBusinessIdByIdentityUserIdAsync(Guid identityUserId);
    Task<UserDescription> AddAsync(UserDescription entity);
    Task UpdateAsync(UserDescription entity);
    Task<UserDescription?> GetByIdentityUserIdAsync(Guid identityUserId);
    Task<IEnumerable<UserDescription>> GetAllAsync();
} 
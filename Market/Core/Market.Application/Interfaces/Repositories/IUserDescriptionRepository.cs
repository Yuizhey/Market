using Market.Domain.Entities;

namespace Market.Application.Interfaces.Repositories;

public interface IUserDescriptionRepository
{
    Task<UserDescription?> GetByIdentityUserIdAsync(Guid identityUserId);
    Task<UserDescription> AddAsync(UserDescription entity);
} 
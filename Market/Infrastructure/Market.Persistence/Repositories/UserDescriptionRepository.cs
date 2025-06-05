using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using Market.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Market.Persistence.Repositories;

public class UserDescriptionRepository : IUserDescriptionRepository
{
    private readonly IGenericRepository<UserDescription> _repository;

    public UserDescriptionRepository(IGenericRepository<UserDescription> repository)
    {
        _repository = repository;
    }

    public async Task<UserDescription?> GetByIdentityUserIdAsync(Guid identityUserId)
    {
        return await _repository.Entities.FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);
    }

    public async Task<UserDescription> AddAsync(UserDescription entity)
    {
        return await _repository.AddAsync(entity);
    }
} 
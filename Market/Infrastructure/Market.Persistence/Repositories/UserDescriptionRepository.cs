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

    public async Task<Guid> GetBusinessIdByIdentityUserIdAsync(Guid identityUserId)
    {
        var user = await _repository.Entities.FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);
        return user.Id;
    }

    public async Task<UserDescription> AddAsync(UserDescription entity)
    {
        return await _repository.AddAsync(entity);
    }
} 
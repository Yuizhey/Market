using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using Market.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Market.Persistence.Repositories;

public class UserDescriptionRepository : IUserDescriptionRepository
{
    private readonly IGenericRepository<UserDescription> _repository;
    private readonly ApplicationDbContext _context;

    public UserDescriptionRepository(
        IGenericRepository<UserDescription> repository,
        ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
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

    public async Task UpdateAsync(UserDescription entity)
    {
        await _context.UserDescriptions
            .Where(x => x.IdentityUserId == entity.IdentityUserId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.FirstName, entity.FirstName)
                .SetProperty(x => x.LastName, entity.LastName)
                .SetProperty(x => x.Gender, entity.Gender)
                .SetProperty(x => x.Phone, entity.Phone)
                .SetProperty(x => x.UpdatedDate, DateTime.UtcNow));
    }

    public async Task<UserDescription?> GetByIdentityUserIdAsync(Guid identityUserId)
    {
        return await _context.UserDescriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);
    }

    public async Task<IEnumerable<UserDescription>> GetAllAsync()
    {
        return await _context.UserDescriptions
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
    }
} 
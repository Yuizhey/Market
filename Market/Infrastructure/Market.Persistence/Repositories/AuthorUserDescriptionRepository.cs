using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using Market.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Market.Persistence.Repositories;

public class AuthorUserDescriptionRepository : IAuthorUserDescriptionRepository
{
    private readonly IGenericRepository<AuthorUserDescription> _repository;
    private readonly ApplicationDbContext _context;

    public AuthorUserDescriptionRepository(
        IGenericRepository<AuthorUserDescription> repository,
        ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<Guid> GetBusinessIdByIdentityUserIdAsync(Guid identityUserId)
    {
        var user =  await _repository.Entities.FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);
        return user.Id;
    }

    public async Task<AuthorUserDescription> AddAsync(AuthorUserDescription entity)
    {
        return await _repository.AddAsync(entity);
    }

    public async Task UpdateAsync(AuthorUserDescription entity)
    {
        await _context.AuthorUserDescriptions
            .Where(x => x.IdentityUserId == entity.IdentityUserId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.FirstName, entity.FirstName)
                .SetProperty(x => x.LastName, entity.LastName)
                .SetProperty(x => x.Country, entity.Country)
                .SetProperty(x => x.Gender, entity.Gender)
                .SetProperty(x => x.Phone, entity.Phone)
                .SetProperty(x => x.HomeAddress, entity.HomeAddress)
                .SetProperty(x => x.Address, entity.Address)
                .SetProperty(x => x.UpdatedDate, DateTime.UtcNow));
    }
} 
using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using Market.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Market.Persistence.Repositories;

public class AuthorUserDescriptionRepository : IAuthorUserDescriptionRepository
{
    private readonly IGenericRepository<AuthorUserDescription> _repository;

    public AuthorUserDescriptionRepository(IGenericRepository<AuthorUserDescription> repository)
    {
        _repository = repository;
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
} 
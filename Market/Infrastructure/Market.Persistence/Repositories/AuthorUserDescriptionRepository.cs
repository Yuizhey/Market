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

    public async Task<AuthorUserDescription?> GetByIdentityUserIdAsync(Guid identityUserId)
    {
        return await _repository.Entities.FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);
    }

    public async Task<AuthorUserDescription> AddAsync(AuthorUserDescription entity)
    {
        return await _repository.AddAsync(entity);
    }
} 
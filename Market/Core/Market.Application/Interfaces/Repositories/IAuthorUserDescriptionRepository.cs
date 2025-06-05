using Market.Domain.Entities;

namespace Market.Application.Interfaces.Repositories;

public interface IAuthorUserDescriptionRepository
{
    Task<AuthorUserDescription?> GetByIdentityUserIdAsync(Guid identityUserId);
    Task<AuthorUserDescription> AddAsync(AuthorUserDescription entity);
} 
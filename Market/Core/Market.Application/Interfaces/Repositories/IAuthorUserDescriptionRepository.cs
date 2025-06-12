using Market.Domain.Entities;

namespace Market.Application.Interfaces.Repositories;

public interface IAuthorUserDescriptionRepository
{
    Task<Guid> GetBusinessIdByIdentityUserIdAsync(Guid identityUserId);
    Task<AuthorUserDescription> AddAsync(AuthorUserDescription entity);
    Task UpdateAsync(AuthorUserDescription entity);
} 
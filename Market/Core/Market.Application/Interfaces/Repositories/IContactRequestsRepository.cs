using Market.Domain.Entities;
using Market.Domain.Enums;

namespace Market.Application.Interfaces.Repositories;

public interface IContactRequestsRepository
{
    Task<ContactRequests> AddAsync(ContactRequests entity);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<ContactRequests>> GetAllAsync(ContactType? type = null);
} 
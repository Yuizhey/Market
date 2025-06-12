using Market.Domain.Entities;

namespace Market.Application.Interfaces.Repositories;

public interface IContactRequestsRepository
{
    Task<ContactRequests> AddAsync(ContactRequests entity);
    Task DeleteAsync(Guid id);
} 
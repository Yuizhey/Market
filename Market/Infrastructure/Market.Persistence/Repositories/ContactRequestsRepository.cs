using Market.Application.Interfaces.Repositories;
using Market.Domain.Entities;
using Market.Domain.Enums;
using Market.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Market.Persistence.Repositories;

public class ContactRequestsRepository : IContactRequestsRepository
{
    private readonly IGenericRepository<ContactRequests> _repository;
    private readonly ApplicationDbContext _context;

    public ContactRequestsRepository(
        IGenericRepository<ContactRequests> repository,
        ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<ContactRequests> AddAsync(ContactRequests entity)
    {
        return await _repository.AddAsync(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.ContactRequests
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<ContactRequests>> GetAllAsync(ContactType? type = null)
    {
        var query = _context.ContactRequests.AsNoTracking();

        if (type.HasValue)
        {
            query = query.Where(x => x.Type == type.Value);
        }

        return await query
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
    }
} 
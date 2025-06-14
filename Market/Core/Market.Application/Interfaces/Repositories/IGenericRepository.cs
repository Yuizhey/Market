using Market.Domain.Common.Interfaces;

namespace Market.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> Entities { get; }
    Task<T> AddAsync(T entity);
    Task<T?> GetByIdAsync(Guid id);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
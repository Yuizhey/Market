using Market.Application.Interfaces.Repositories;
using Market.Domain.Common;
using Market.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Market.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseAuditableEntity
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<T> _dbSet;
 
    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }
 
    public IQueryable<T> Entities => _dbSet;
 
    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }
 
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }
 
    public async Task<T> UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return entity;
    }
 
    public async Task DeleteAsync(T entity)
    { 
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
 
    public async Task<List<T>> GetAllAsync()
    {
        return await _dbContext
            .Set<T>()
            .ToListAsync();
    }
}
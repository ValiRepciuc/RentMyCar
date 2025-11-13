using System.Linq.Expressions;
using Domain.Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Delete(TEntity entity, bool removeFromDb = false);
    void DeleteRange(IEnumerable<TEntity> entities, bool removeFromDb = false);
    Task<TEntity> GetByIdAsync(Guid id, bool includeDeleted = false);
    Task<List<TEntity>> GetAllByIdsAsync(IEnumerable<Guid> ids, bool includeDeleted = false);
    Task<Dictionary<TKey, TEntity>> GetAllByIdsAsync<TKey>(IEnumerable<Guid> ids, Func<TEntity, TKey> keySelector, bool includeDeleted = false);
    Task<List<TEntity>> GetAllAsync(bool includeDeleted = false);
    Task<List<TEntity>> GetAllAsync<TKey>(Expression<Func<TEntity, TKey>> orderBy, bool descending = false);
    Task<bool> ExistsAsync(Guid id, bool includeDeleted = false);
    Task<bool> ExistsRangeAsync(IEnumerable<Guid> ids, bool includeDeleted = false);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);
    Task<bool> AnyAsync();
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<TEntity, bool>> expression);
    void Update(TEntity entity);

    
}

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
{
    protected readonly DbSet<TEntity> _dbSet;
    private readonly DatabaseContext _context;

    protected BaseRepository(DatabaseContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    protected IQueryable<TEntity> Get(bool includeDeleted = false)
    {
        return includeDeleted
            ? _dbSet.IgnoreQueryFilters()
            : _dbSet;
    }
    
    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Add(TEntity entity)
    {
        _context.Add(entity);
    }
    
    public void AddRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            Add(entity);
    }
    
    public void Delete(TEntity entity, bool removeFromDb = false)
    {
        entity.DeletedAt = DateTime.UtcNow;
    }

    public void DeleteRange(IEnumerable<TEntity> entities, bool removeFromDb = false)
    {
        foreach (var entity in entities)
            Delete(entity, removeFromDb);
    }

    public async Task<TEntity> GetByIdAsync(Guid id, bool includeDeleted = false)
    {
        return await Get(includeDeleted).FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<TEntity>> GetAllByIdsAsync(IEnumerable<Guid> ids, bool includeDeleted = false)
    {
        return await Get(includeDeleted).Where(e => ids.Contains(e.Id)).ToListAsync();
    }
    
    public async Task<Dictionary<TKey, TEntity>> GetAllByIdsAsync<TKey>(IEnumerable<Guid> ids, Func<TEntity, TKey> keySelector, bool includeDeleted = false)
    {
        return await Get(includeDeleted).Where(e => ids.Contains(e.Id)).ToDictionaryAsync(keySelector);
    }

    public async Task<List<TEntity>> GetAllAsync(bool includeDeleted = false)
    {
        return await Get(includeDeleted).ToListAsync();
    }
    
    public async Task<List<TEntity>> GetAllAsync<TKey>(Expression<Func<TEntity, TKey>> orderBy, bool descending = false)
    {
        return descending
            ? await Get().OrderByDescending(orderBy).ToListAsync()
            : await Get().OrderBy(orderBy).ToListAsync();
    }
    public async Task<bool> ExistsAsync(Guid id, bool includeDeleted = false)
    {
        return await Get(includeDeleted).AnyAsync(e => e.Id == id);
    }

    public async Task<bool> ExistsRangeAsync(IEnumerable<Guid> ids, bool includeDeleted = false)
    {
        return await Get(includeDeleted).Where(e => ids.Contains(e.Id)).CountAsync() == ids.Count();
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await Get().AnyAsync(expression);
    }
    
    public async Task<bool> AnyAsync()
    {
        return await Get().AnyAsync();
    }
    
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await Get().AnyAsync(expression);
    }
    
    public async Task<int> CountAsync()
    {
        return await Get().CountAsync();
    }
    
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await Get().CountAsync(expression);
    }
}
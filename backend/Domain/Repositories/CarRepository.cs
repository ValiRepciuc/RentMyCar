using Domain.Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface ICarRepository : IBaseRepository<Car>
{
    Task<List<Car>> QueryAsync(Func<IQueryable<Car>, IQueryable<Car>> query);
}

public class CarRepository : BaseRepository<Car>, ICarRepository
{
    private readonly DatabaseContext _context;
    public CarRepository(DatabaseContext context) : base(context)
    {
        _context = context;
    }

    private IQueryable<Car> GetQueryWithOwner(bool includeDeleted = false)
    {
        var query = includeDeleted
            ? _context.Car.IgnoreQueryFilters()
            : _context.Car;
        
        return query.Include(c => c.Owner);
    }

    public async Task<List<Car>> QueryAsync(Func<IQueryable<Car>, IQueryable<Car>> func)
    {
        IQueryable<Car> query = GetQueryWithOwner();
        query = func(query);
        return await query.ToListAsync();
    }

    public new async Task<Car> GetByIdAsync(Guid id, bool includeDeleted = false)
    {
        return await GetQueryWithOwner(includeDeleted).FirstOrDefaultAsync(e => e.Id == id);
    }
}
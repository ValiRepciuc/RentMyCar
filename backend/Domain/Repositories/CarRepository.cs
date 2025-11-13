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

    public async Task<List<Car>> QueryAsync(Func<IQueryable<Car>, IQueryable<Car>> func)
    {
        IQueryable<Car> query = _context.Car.Include(c => c.Owner);
        query = func(query);
        return await query.ToListAsync();
    }
}
using Domain.Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface IBookingRepository : IBaseRepository<Booking>
{
    Task<List<Booking>> QueryAsync(Func<IQueryable<Booking>, IQueryable<Booking>> query);
    Task<List<Booking>> GetAllFullAsync();
    Task<Booking?> GetByIdFullAsync(Guid id);


    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        private readonly DatabaseContext _context;

        public BookingRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Booking>> QueryAsync(Func<IQueryable<Booking>, IQueryable<Booking>> query)
        {
            IQueryable<Booking> baseQuery = _context.Booking
                .Include(b => b.Car)
                .Include(b => b.Renter)
                .Include(b => b.Review);

            baseQuery = query(baseQuery);
            return await baseQuery.ToListAsync();
        }

        public async Task<List<Booking>> GetAllFullAsync()
        {
            return await _context.Booking
                .Include(b => b.Car)
                .Include(b => b.Renter)
                .Include(b => b.Review)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdFullAsync(Guid id)
        {
            return await _context.Booking
                .Include(b => b.Car)
                .Include(b => b.Renter)
                .Include(b => b.Review)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
using Domain.Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface IReviewRepository : IBaseRepository<Review>
{
    Task<Review?> GetByBookingIdAsync(Guid bookingId);
}

public class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    private readonly DatabaseContext _context;
    public ReviewRepository(DatabaseContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Review?> GetByBookingIdAsync(Guid bookingId)
    {
        return await _context.Review.FirstOrDefaultAsync(b => b.BookingId == bookingId);
    }
}
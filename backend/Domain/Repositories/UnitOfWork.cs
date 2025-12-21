using Domain.Database;

namespace Domain.Repositories;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    ICarRepository Cars { get; }
    IBookingRepository Bookings { get; }
    IReviewRepository Reviews { get; }
  
    Task<bool> SaveChangesAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _context;
   
    public UnitOfWork(DatabaseContext context)
    {
        _context = context;
        
        Users = new UserRepository(_context);
        Cars = new CarRepository(_context);
        Bookings = new IBookingRepository.BookingRepository(_context);
        Reviews = new ReviewRepository(_context);
       
    }

    public IUserRepository Users
    { get; }

    public ICarRepository Cars { get; }
    public IBookingRepository Bookings { get; }
    public IReviewRepository Reviews { get; }

    public async Task<bool> SaveChangesAsync()
    {
        var save = await _context.SaveChangesAsync();
        return save >= 0;
    }
}

namespace Domain.Entities;

public class Review : BaseEntity
{
    public Guid BookingId { get; set; }
    public Booking Booking { get; set; } = null!;
    
    public string AuthorId { get; set; }
    public AppUser Author { get; set; } = null!;
    
    public int Rating { get; set; }
    public string Comment { get; set; } = String.Empty;
}
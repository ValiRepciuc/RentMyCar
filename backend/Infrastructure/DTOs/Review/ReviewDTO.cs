namespace Infrastructure.DTOs.Review;

public class ReviewDTO
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public string AuthorId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}
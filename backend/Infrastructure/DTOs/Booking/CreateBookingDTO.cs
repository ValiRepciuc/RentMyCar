namespace Infrastructure.DTOs.Booking;

public class CreateBookingDTO
{
    public Guid CarId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}

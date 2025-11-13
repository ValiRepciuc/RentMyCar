namespace Infrastructure.DTOs.Booking;

public class UpdateBookingDTO
{
    public Guid CarId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
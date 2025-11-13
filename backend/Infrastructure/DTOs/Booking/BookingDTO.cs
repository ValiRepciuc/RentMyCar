namespace Infrastructure.DTOs.Booking;

public class BookingDTO
{
    public Guid Id { get; set; }
    
    public Guid CarId { get; set; }
    public string CarBrand { get; set; } = null!;
    public string CarModel { get; set; } = null!;
    
    public string RenterId { get; set; } = null!;
    public string RenterName { get; set; } = null!;
    
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int TotalPrice { get; set; }
    public string Status { get; set; } = null!;
}
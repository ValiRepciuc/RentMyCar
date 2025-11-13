namespace Domain.Entities;

public class Booking : BaseEntity
{
    public Guid CarId { get; set; }
    public Car Car { get; set; } = null!;
    
    public string RenterId { get; set; }
    public AppUser Renter { get; set; } = null!;
    
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int TotalPrice { get; set; }
    public string Status { get; set; } = "Pending";
    
    public Review? Review { get; set; }
}
namespace Domain.Entities;

public class Car : BaseEntity
{
    public string Brand  { get; set; } = String.Empty;
    public string Model { get; set; } = String.Empty;
    public int Year { get; set; }
    public int PricePerDay { get; set; }
    public string City { get; set; } = String.Empty;
    public string FuelType { get; set; } = "Petrol";
    public string Transmission { get; set; } = "Manual";
    public bool IsActive { get; set; } = true;
    
    // New fields for frontend integration
    public string Description { get; set; } = String.Empty;
    public string Features { get; set; } = String.Empty; // JSON array stored as string
    public string ImageUrl { get; set; } = String.Empty;
    public string ImageUrls { get; set; } = String.Empty; // JSON array stored as string
    public int Seats { get; set; } = 5;
    public double Rating { get; set; } = 0.0;
    public int ReviewCount { get; set; } = 0;
    
    public string OwnerId { get; set; } = String.Empty;
    public AppUser Owner { get; set; }
    
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
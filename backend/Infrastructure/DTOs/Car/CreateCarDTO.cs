namespace Infrastructure.DTOs.Car;

public class CreateCarDTO
{
    public  string Brand { get; set; } = String.Empty;
    public string Model { get; set; } = String.Empty;
    public int Year { get; set; }
    public int PricePerDay { get; set; }
    public string City { get; set; } = String.Empty;
    public string FuelType { get; set; } = "Petrol";
    public string Transmission { get; set; } = "Manual";
    public bool IsActive { get; set; } = true;
    
    // Image fields
    public string ImageUrl { get; set; } = String.Empty;
    public List<string> ImageUrls { get; set; } = new List<string>();
    
    // Extended information
    public string Description { get; set; } = String.Empty;
    public List<string> Features { get; set; } = new List<string>();
    public int Seats { get; set; } = 5;
}
using Domain.Entities;

namespace Infrastructure.DTOs.Car;

public class CarDTO
{
    public Guid Id { get; set; }
    public  string Brand { get; set; } = String.Empty;
    public string Model { get; set; } = String.Empty;
    public int Year { get; set; }
    public int PricePerDay { get; set; }
    public string City { get; set; } = String.Empty;
    public string FuelType { get; set; } = "Petrol";
    public string Transmission { get; set; } = "Manual";
    public bool IsActive { get; set; } = true;
    
    // New fields for frontend integration
    public string Description { get; set; } = String.Empty;
    public string[] Features { get; set; } = Array.Empty<string>();
    public string ImageUrl { get; set; } = String.Empty;
    public string[] ImageUrls { get; set; } = Array.Empty<string>();
    public int Seats { get; set; } = 5;
    public double Rating { get; set; } = 0.0;
    public int ReviewCount { get; set; } = 0;
    
    public string OwnerId { get; set; }
    public string OwnerName { get; set; }
    
}
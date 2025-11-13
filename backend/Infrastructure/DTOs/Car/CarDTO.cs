using Domain.Entities;

namespace Infrastructure.DTOs.Car;

public class CarDTO
{
    public  string Brand { get; set; } = String.Empty;
    public string Model { get; set; } = String.Empty;
    public int Year { get; set; }
    public int PricePerDay { get; set; }
    public string City { get; set; } = String.Empty;
    public string FuelType { get; set; } = "Petrol";
    public string Transmission { get; set; } = "Manual";
    public bool IsActive { get; set; } = true;
    public string OwnerId { get; set; }
    public string OwnerName { get; set; }
    
}
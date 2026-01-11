using Domain.Entities;
using Infrastructure.DTOs.Car;
using System.Text.Json;

namespace Infrastructure.Mappers;

public static class CarMapper
{
    public static CarDTO ToCarDTO(this Car car)
    {
        return new CarDTO
        {
            Id = car.Id,
            Brand = car.Brand,
            Model = car.Model,
            Year = car.Year,
            PricePerDay = car.PricePerDay,
            City = car.City,
            FuelType = car.FuelType,
            Transmission = car.Transmission,
            IsActive = car.IsActive,
            OwnerId = car.OwnerId,
            OwnerName = $"{car.Owner.FristName} {car.Owner.LastName}",
            ImageUrl = car.ImageUrl,
            ImageUrls = DeserializeJsonArray(car.ImageUrls),
            Description = car.Description,
            Features = DeserializeJsonArray(car.Features),
            Seats = car.Seats,
            Rating = car.Rating,
            ReviewCount = car.ReviewCount
        };
    }

    public static Car ToCarFromCreate(this CreateCarDTO carDto)
    {
        return new Car
        {
            Brand = carDto.Brand,
            Model = carDto.Model,
            Year = carDto.Year,
            PricePerDay = carDto.PricePerDay,
            City = carDto.City,
            FuelType = carDto.FuelType,
            Transmission = carDto.Transmission,
            IsActive = carDto.IsActive,
            ImageUrl = carDto.ImageUrl,
            ImageUrls = SerializeJsonArray(carDto.ImageUrls),
            Description = carDto.Description,
            Features = SerializeJsonArray(carDto.Features),
            Seats = carDto.Seats,
            Rating = 0.0,
            ReviewCount = 0
        };
    }

    public static void ToCarFromUpdate(this UpdateCarDTO carDto, Car existingCar)
    {
        existingCar.Brand = carDto.Brand;
        existingCar.Model = carDto.Model;
        existingCar.Year = carDto.Year;
        existingCar.PricePerDay = carDto.PricePerDay;
        existingCar.City = carDto.City;
        existingCar.FuelType = carDto.FuelType;
        existingCar.Transmission = carDto.Transmission;
        existingCar.IsActive = carDto.IsActive;
        
        if (carDto.ImageUrl != null)
            existingCar.ImageUrl = carDto.ImageUrl;
        
        if (carDto.ImageUrls != null)
            existingCar.ImageUrls = SerializeJsonArray(carDto.ImageUrls);
        
        if (carDto.Description != null)
            existingCar.Description = carDto.Description;
        
        if (carDto.Features != null)
            existingCar.Features = SerializeJsonArray(carDto.Features);
        
        if (carDto.Seats.HasValue)
            existingCar.Seats = carDto.Seats.Value;
    }
    
    private static string SerializeJsonArray(List<string> list)
    {
        return JsonSerializer.Serialize(list);
    }
    
    private static List<string> DeserializeJsonArray(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }
}
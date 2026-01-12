using System.Text.Json;
using Domain.Entities;
using Infrastructure.DTOs.Car;

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
            Description = car.Description,
            Features = string.IsNullOrEmpty(car.Features) ? Array.Empty<string>() : JsonSerializer.Deserialize<string[]>(car.Features) ?? Array.Empty<string>(),
            ImageUrl = car.ImageUrl,
            ImageUrls = string.IsNullOrEmpty(car.ImageUrls) ? Array.Empty<string>() : JsonSerializer.Deserialize<string[]>(car.ImageUrls) ?? Array.Empty<string>(),
            Seats = car.Seats,
            Rating = car.Rating,
            ReviewCount = car.ReviewCount,
            OwnerId = car.OwnerId,
            OwnerName = $"{car.Owner.FristName} {car.Owner.LastName}"
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
            Description = carDto.Description,
            Features = carDto.Features != null && carDto.Features.Any() ? JsonSerializer.Serialize(carDto.Features) : String.Empty,
            ImageUrl = carDto.ImageUrl,
            ImageUrls = carDto.ImageUrls != null && carDto.ImageUrls.Any() ? JsonSerializer.Serialize(carDto.ImageUrls) : String.Empty,
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
        existingCar.Description = carDto.Description;
        existingCar.Features = carDto.Features != null && carDto.Features.Any() ? JsonSerializer.Serialize(carDto.Features) : String.Empty;
        existingCar.ImageUrl = carDto.ImageUrl;
        existingCar.ImageUrls = carDto.ImageUrls != null && carDto.ImageUrls.Any() ? JsonSerializer.Serialize(carDto.ImageUrls) : String.Empty;
        existingCar.Seats = carDto.Seats;
    }
}
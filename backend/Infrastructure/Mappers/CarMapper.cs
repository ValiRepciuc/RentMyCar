using Domain.Entities;
using Infrastructure.DTOs.Car;

namespace Infrastructure.Mappers;

public static class CarMapper
{
    public static CarDTO ToCarDTO(this Car car)
    {
        return new CarDTO
        {
            Brand = car.Brand,
            Model = car.Model,
            Year = car.Year,
            PricePerDay = car.PricePerDay,
            City = car.City,
            FuelType = car.FuelType,
            Transmission = car.Transmission,
            IsActive = car.IsActive,
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

    }
}
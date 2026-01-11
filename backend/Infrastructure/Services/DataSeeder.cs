using Domain.Database;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public interface IDataSeeder
{
    Task SeedAsync();
}

public class DataSeeder : IDataSeeder
{
    private readonly DatabaseContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(DatabaseContext context, UserManager<AppUser> userManager, ILogger<DataSeeder> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Starting database seeding...");

        // Check if data already exists
        if (await _context.Users.AnyAsync(u => u.UserName == "john.doe"))
        {
            _logger.LogInformation("Database already seeded. Skipping...");
            return;
        }

        // Seed Users
        var users = await SeedUsersAsync();
        
        // Seed Cars
        await SeedCarsAsync(users);

        _logger.LogInformation("Database seeding completed successfully!");
    }

    private async Task<Dictionary<string, AppUser>> SeedUsersAsync()
    {
        _logger.LogInformation("Seeding users...");
        
        var userDictionary = new Dictionary<string, AppUser>();
        
        var usersData = new[]
        {
            new { MockId = "user-1", UserName = "john.doe", Email = "john@example.com", FirstName = "John", LastName = "Doe", City = "New York", Role = "User" },
            new { MockId = "user-2", UserName = "sarah.johnson", Email = "sarah@example.com", FirstName = "Sarah", LastName = "Johnson", City = "Los Angeles", Role = "Owner" },
            new { MockId = "user-3", UserName = "mike.chen", Email = "mike@example.com", FirstName = "Mike", LastName = "Chen", City = "San Francisco", Role = "Owner" },
            new { MockId = "user-4", UserName = "emily.davis", Email = "emily@example.com", FirstName = "Emily", LastName = "Davis", City = "Chicago", Role = "Owner" }
        };

        foreach (var userData in usersData)
        {
            var user = new AppUser
            {
                UserName = userData.UserName,
                Email = userData.Email,
                FristName = userData.FirstName,
                LastName = userData.LastName,
                City = userData.City,
                EmailConfirmed = true,
                LockoutEnabled = false
            };

            var result = await _userManager.CreateAsync(user, "Password123!");
            
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, userData.Role);
                userDictionary[userData.MockId] = user;
                _logger.LogInformation($"Created user: {userData.UserName} with role {userData.Role}");
            }
            else
            {
                _logger.LogError($"Failed to create user {userData.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        return userDictionary;
    }

    private async Task SeedCarsAsync(Dictionary<string, AppUser> users)
    {
        _logger.LogInformation("Seeding cars...");
        
        var carsData = new[]
        {
            new { Brand = "Toyota", Model = "Camry", Year = 2022, PricePerDay = 65, City = "New York", FuelType = "Petrol", Transmission = "Automatic", OwnerId = "user-2", ImageUrl = "https://images.pexels.com/photos/1149831/pexels-photo-1149831.jpeg?auto=compress&cs=tinysrgb&w=800" },
            new { Brand = "Honda", Model = "CR-V", Year = 2023, PricePerDay = 75, City = "Los Angeles", FuelType = "Hybrid", Transmission = "Automatic", OwnerId = "user-2", ImageUrl = "https://images.pexels.com/photos/164634/pexels-photo-164634.jpeg?auto=compress&cs=tinysrgb&w=800" },
            new { Brand = "Tesla", Model = "Model 3", Year = 2023, PricePerDay = 95, City = "San Francisco", FuelType = "Electric", Transmission = "Automatic", OwnerId = "user-3", ImageUrl = "https://images.pexels.com/photos/27786289/pexels-photo-27786289.jpeg?auto=compress&cs=tinysrgb&w=800" },
            new { Brand = "BMW", Model = "X5", Year = 2022, PricePerDay = 110, City = "Chicago", FuelType = "Petrol", Transmission = "Automatic", OwnerId = "user-4", ImageUrl = "https://images.pexels.com/photos/909907/pexels-photo-909907.jpeg?auto=compress&cs=tinysrgb&w=800" },
            new { Brand = "Ford", Model = "Mustang", Year = 2023, PricePerDay = 120, City = "Miami", FuelType = "Petrol", Transmission = "Manual", OwnerId = "user-4", ImageUrl = "https://images.pexels.com/photos/544542/pexels-photo-544542.jpeg?auto=compress&cs=tinysrgb&w=800" },
            new { Brand = "Jeep", Model = "Wrangler", Year = 2022, PricePerDay = 85, City = "Denver", FuelType = "Petrol", Transmission = "Automatic", OwnerId = "user-3", ImageUrl = "https://images.pexels.com/photos/1545743/pexels-photo-1545743.jpeg?auto=compress&cs=tinysrgb&w=800" },
            new { Brand = "Mercedes-Benz", Model = "C-Class", Year = 2023, PricePerDay = 100, City = "New York", FuelType = "Petrol", Transmission = "Automatic", OwnerId = "user-2", ImageUrl = "https://images.pexels.com/photos/170811/pexels-photo-170811.jpeg?auto=compress&cs=tinysrgb&w=800" },
            new { Brand = "Chevrolet", Model = "Tahoe", Year = 2022, PricePerDay = 90, City = "Austin", FuelType = "Petrol", Transmission = "Automatic", OwnerId = "user-4", ImageUrl = "https://images.pexels.com/photos/627678/pexels-photo-627678.jpeg?auto=compress&cs=tinysrgb&w=800" }
        };

        foreach (var carData in carsData)
        {
            if (!users.ContainsKey(carData.OwnerId))
            {
                _logger.LogWarning($"Owner with ID {carData.OwnerId} not found. Skipping car {carData.Brand} {carData.Model}");
                continue;
            }

            var car = new Car
            {
                Brand = carData.Brand,
                Model = carData.Model,
                Year = carData.Year,
                PricePerDay = carData.PricePerDay,
                City = carData.City,
                FuelType = carData.FuelType,
                Transmission = carData.Transmission,
                OwnerId = users[carData.OwnerId].Id,
                IsActive = true
            };

            _context.Car.Add(car);
            _logger.LogInformation($"Added car: {carData.Brand} {carData.Model}");
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Cars seeded successfully!");
    }
}

using System.Text.Json;
using Domain.Database;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Presentation;

public static class SeedData
{
    public static async Task SeedAsync(DatabaseContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Ensure database is created
        await context.Database.MigrateAsync();
        
        Console.WriteLine("Starting data seeding...");
        
        // Check if data already exists
        if (await context.Car.AnyAsync())
        {
            Console.WriteLine("Database already contains data. Skipping seed.");
            return;
        }
        
        // Create users from mockdata
        var users = new Dictionary<string, AppUser>();
        
        var owner1 = new AppUser
        {
            UserName = "sarah@example.com",
            Email = "sarah@example.com",
            FristName = "Sarah",
            LastName = "Johnson",
            City = "New York",
            EmailConfirmed = true
        };
        var result1 = await userManager.CreateAsync(owner1, "Password123!");
        if (result1.Succeeded)
        {
            await userManager.AddToRoleAsync(owner1, "Owner");
            users["user-2"] = owner1;
            Console.WriteLine($"Created user: {owner1.Email}");
        }
        
        var owner2 = new AppUser
        {
            UserName = "mike@example.com",
            Email = "mike@example.com",
            FristName = "Mike",
            LastName = "Chen",
            City = "San Francisco",
            EmailConfirmed = true
        };
        var result2 = await userManager.CreateAsync(owner2, "Password123!");
        if (result2.Succeeded)
        {
            await userManager.AddToRoleAsync(owner2, "Owner");
            users["user-3"] = owner2;
            Console.WriteLine($"Created user: {owner2.Email}");
        }
        
        var owner3 = new AppUser
        {
            UserName = "emily@example.com",
            Email = "emily@example.com",
            FristName = "Emily",
            LastName = "Davis",
            City = "Chicago",
            EmailConfirmed = true
        };
        var result3 = await userManager.CreateAsync(owner3, "Password123!");
        if (result3.Succeeded)
        {
            await userManager.AddToRoleAsync(owner3, "Owner");
            users["user-4"] = owner3;
            Console.WriteLine($"Created user: {owner3.Email}");
        }
        
        // Create cars with data from frontend mockdata
        var cars = new List<Car>
        {
            new Car
            {
                Brand = "Toyota",
                Model = "Camry",
                Year = 2022,
                City = "New York",
                PricePerDay = 65,
                Description = "Reliable and comfortable sedan perfect for city driving and road trips.",
                Features = JsonSerializer.Serialize(new[] { "Bluetooth", "Backup Camera", "Apple CarPlay", "Cruise Control" }),
                ImageUrl = "http://localhost:5039/images/toyota.avif",
                ImageUrls = JsonSerializer.Serialize(new[] { "http://localhost:5039/images/toyota.avif", "http://localhost:5039/images/toyota2.webp" }),
                Transmission = "Automatic",
                FuelType = "Gasoline",
                Seats = 5,
                Rating = 4.8,
                ReviewCount = 24,
                OwnerId = users["user-2"].Id,
                IsActive = true
            },
            new Car
            {
                Brand = "Honda",
                Model = "CR-V",
                Year = 2023,
                City = "Los Angeles",
                PricePerDay = 75,
                Description = "Spacious SUV with excellent fuel economy and modern safety features.",
                Features = JsonSerializer.Serialize(new[] { "All-Wheel Drive", "Lane Assist", "Sunroof", "Heated Seats" }),
                ImageUrl = "http://localhost:5039/images/honda.jpeg",
                ImageUrls = JsonSerializer.Serialize(new[] { "http://localhost:5039/images/honda.jpeg" }),
                Transmission = "Automatic",
                FuelType = "Hybrid",
                Seats = 5,
                Rating = 4.9,
                ReviewCount = 18,
                OwnerId = users["user-2"].Id,
                IsActive = true
            },
            new Car
            {
                Brand = "Tesla",
                Model = "Model 3",
                Year = 2023,
                City = "San Francisco",
                PricePerDay = 95,
                Description = "Experience the future with this fully electric sedan. Autopilot included!",
                Features = JsonSerializer.Serialize(new[] { "Autopilot", "Premium Sound", "Glass Roof", "Supercharger Access" }),
                ImageUrl = "http://localhost:5039/images/tesla.jpg",
                ImageUrls = JsonSerializer.Serialize(new[] { "http://localhost:5039/images/tesla.jpg" }),
                Transmission = "Automatic",
                FuelType = "Electric",
                Seats = 5,
                Rating = 5.0,
                ReviewCount = 32,
                OwnerId = users["user-3"].Id,
                IsActive = true
            },
            new Car
            {
                Brand = "BMW",
                Model = "X5",
                Year = 2022,
                City = "Chicago",
                PricePerDay = 110,
                Description = "Luxury SUV with premium features and powerful performance.",
                Features = JsonSerializer.Serialize(new[] { "Leather Seats", "Premium Audio", "Panoramic Roof", "Navigation" }),
                ImageUrl = "http://localhost:5039/images/bmw.webp",
                ImageUrls = JsonSerializer.Serialize(new[] { "http://localhost:5039/images/bmw.webp", "http://localhost:5039/images/bmw2.jpeg" }),
                Transmission = "Automatic",
                FuelType = "Gasoline",
                Seats = 7,
                Rating = 4.7,
                ReviewCount = 15,
                OwnerId = users["user-4"].Id,
                IsActive = true
            },
            new Car
            {
                Brand = "Ford",
                Model = "Mustang",
                Year = 2023,
                City = "Miami",
                PricePerDay = 120,
                Description = "Iconic sports car for those who want to make a statement.",
                Features = JsonSerializer.Serialize(new[] { "Sport Mode", "Premium Sound", "Performance Package", "Rear Spoiler" }),
                ImageUrl = "https://images.pexels.com/photos/544542/pexels-photo-544542.jpeg?auto=compress&cs=tinysrgb&w=800",
                ImageUrls = JsonSerializer.Serialize(new[] { "https://images.pexels.com/photos/544542/pexels-photo-544542.jpeg?auto=compress&cs=tinysrgb&w=800" }),
                Transmission = "Manual",
                FuelType = "Gasoline",
                Seats = 4,
                Rating = 4.9,
                ReviewCount = 21,
                OwnerId = users["user-4"].Id,
                IsActive = true
            },
            new Car
            {
                Brand = "Jeep",
                Model = "Wrangler",
                Year = 2022,
                City = "Denver",
                PricePerDay = 85,
                Description = "Adventure-ready SUV perfect for off-road trips and outdoor activities.",
                Features = JsonSerializer.Serialize(new[] { "4x4", "Removable Top", "Off-Road Tires", "Tow Package" }),
                ImageUrl = "http://localhost:5039/images/jeep.jpg",
                ImageUrls = JsonSerializer.Serialize(new[] { "http://localhost:5039/images/jeep.jpg" }),
                Transmission = "Automatic",
                FuelType = "Gasoline",
                Seats = 5,
                Rating = 4.6,
                ReviewCount = 19,
                OwnerId = users["user-3"].Id,
                IsActive = true
            },
            new Car
            {
                Brand = "Mercedes-Benz",
                Model = "C-Class",
                Year = 2023,
                City = "New York",
                PricePerDay = 100,
                Description = "Elegant luxury sedan with cutting-edge technology and comfort.",
                Features = JsonSerializer.Serialize(new[] { "Ambient Lighting", "Massage Seats", "Voice Control", "Advanced Safety" }),
                ImageUrl = "http://localhost:5039/images/mercedes.avif",
                ImageUrls = JsonSerializer.Serialize(new[] { "http://localhost:5039/images/mercedes.avif" }),
                Transmission = "Automatic",
                FuelType = "Gasoline",
                Seats = 5,
                Rating = 4.8,
                ReviewCount = 27,
                OwnerId = users["user-2"].Id,
                IsActive = false // Was marked as not available in mockdata
            },
            new Car
            {
                Brand = "Chevrolet",
                Model = "Tahoe",
                Year = 2022,
                City = "Austin",
                PricePerDay = 90,
                Description = "Full-size SUV with plenty of space for families and luggage.",
                Features = JsonSerializer.Serialize(new[] { "Third Row", "WiFi Hotspot", "Towing Capacity", "Rear Entertainment" }),
                ImageUrl = "http://localhost:5039/images/chevrolet.jpg",
                ImageUrls = JsonSerializer.Serialize(new[] { "http://localhost:5039/images/chevrolet.jpg", "http://localhost:5039/images/chevrolet2.webp" }),
                Transmission = "Automatic",
                FuelType = "Gasoline",
                Seats = 8,
                Rating = 4.5,
                ReviewCount = 13,
                OwnerId = users["user-4"].Id,
                IsActive = true
            }
        };
        
        await context.Car.AddRangeAsync(cars);
        await context.SaveChangesAsync();
        
        Console.WriteLine($"Successfully seeded {cars.Count} cars.");
        Console.WriteLine("Data seeding completed!");
    }
}

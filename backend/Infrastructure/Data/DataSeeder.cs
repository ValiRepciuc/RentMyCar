using Domain.Database;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataSeeder
{
    private readonly DatabaseContext _context;
    private readonly UserManager<AppUser> _userManager;

    public DataSeeder(DatabaseContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task SeedAsync()
    {
        // Check if data already exists
        if (await _context.Car.AnyAsync())
        {
            Console.WriteLine("Database already contains data. Skipping seed.");
            return;
        }

        Console.WriteLine("Starting database seeding...");

        // Seed users
        var users = await SeedUsersAsync();
        Console.WriteLine($"Seeded {users.Count} users");

        // Seed cars
        var cars = await SeedCarsAsync(users);
        Console.WriteLine($"Seeded {cars.Count} cars");

        // Seed bookings
        var bookings = await SeedBookingsAsync(cars, users);
        Console.WriteLine($"Seeded {bookings.Count} bookings");

        // Seed reviews
        var reviews = await SeedReviewsAsync(bookings, users);
        Console.WriteLine($"Seeded {reviews.Count} reviews");

        Console.WriteLine("Database seeding completed successfully!");
    }

    private async Task<Dictionary<string, AppUser>> SeedUsersAsync()
    {
        var userDict = new Dictionary<string, AppUser>();

        var usersData = new[]
        {
            new { Id = "user-1", Email = "john@example.com", FirstName = "John", LastName = "Doe", UserName = "johndoe", Role = "User", City = "New York" },
            new { Id = "user-2", Email = "sarah@example.com", FirstName = "Sarah", LastName = "Johnson", UserName = "sarahjohnson", Role = "Owner", City = "Los Angeles" },
            new { Id = "user-3", Email = "mike@example.com", FirstName = "Mike", LastName = "Chen", UserName = "mikechen", Role = "Owner", City = "San Francisco" },
            new { Id = "user-4", Email = "emily@example.com", FirstName = "Emily", LastName = "Davis", UserName = "emilydavis", Role = "Owner", City = "Chicago" }
        };

        foreach (var userData in usersData)
        {
            var existingUser = await _userManager.FindByEmailAsync(userData.Email);
            if (existingUser == null)
            {
                var user = new AppUser
                {
                    Email = userData.Email,
                    UserName = userData.UserName,
                    FristName = userData.FirstName,
                    LastName = userData.LastName,
                    City = userData.City,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "Password123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, userData.Role);
                    userDict[userData.Id] = user;
                }
            }
            else
            {
                userDict[userData.Id] = existingUser;
            }
        }

        return userDict;
    }

    private async Task<Dictionary<string, Car>> SeedCarsAsync(Dictionary<string, AppUser> users)
    {
        var carDict = new Dictionary<string, Car>();

        var carsData = new[]
        {
            new { Id = "car-1", OwnerId = "user-2", Brand = "Toyota", Model = "Camry", Year = 2022, City = "New York", PricePerDay = 65, FuelType = "Gasoline", Transmission = "Automatic", Image = "/images/toyota.avif" },
            new { Id = "car-2", OwnerId = "user-2", Brand = "Honda", Model = "CR-V", Year = 2023, City = "Los Angeles", PricePerDay = 75, FuelType = "Hybrid", Transmission = "Automatic", Image = "/images/honda.jpeg" },
            new { Id = "car-3", OwnerId = "user-3", Brand = "Tesla", Model = "Model 3", Year = 2023, City = "San Francisco", PricePerDay = 95, FuelType = "Electric", Transmission = "Automatic", Image = "/images/tesla.jpg" },
            new { Id = "car-4", OwnerId = "user-4", Brand = "BMW", Model = "X5", Year = 2022, City = "Chicago", PricePerDay = 110, FuelType = "Gasoline", Transmission = "Automatic", Image = "/images/bmw.webp" },
            new { Id = "car-5", OwnerId = "user-4", Brand = "Ford", Model = "Mustang", Year = 2023, City = "Miami", PricePerDay = 120, FuelType = "Gasoline", Transmission = "Manual", Image = "https://images.pexels.com/photos/544542/pexels-photo-544542.jpeg?auto=compress&cs=tinysrgb&w=800" },
            new { Id = "car-6", OwnerId = "user-3", Brand = "Jeep", Model = "Wrangler", Year = 2022, City = "Denver", PricePerDay = 85, FuelType = "Gasoline", Transmission = "Automatic", Image = "/images/jeep.jpg" },
            new { Id = "car-7", OwnerId = "user-2", Brand = "Mercedes-Benz", Model = "C-Class", Year = 2023, City = "New York", PricePerDay = 100, FuelType = "Gasoline", Transmission = "Automatic", Image = "/images/mercedes.avif" },
            new { Id = "car-8", OwnerId = "user-4", Brand = "Chevrolet", Model = "Tahoe", Year = 2022, City = "Austin", PricePerDay = 90, FuelType = "Gasoline", Transmission = "Automatic", Image = "/images/chevrolet.jpg" }
        };

        foreach (var carData in carsData)
        {
            if (users.TryGetValue(carData.OwnerId, out var owner))
            {
                var car = new Car
                {
                    Id = Guid.NewGuid(),
                    Brand = carData.Brand,
                    Model = carData.Model,
                    Year = carData.Year,
                    City = carData.City,
                    PricePerDay = carData.PricePerDay,
                    FuelType = carData.FuelType,
                    Transmission = carData.Transmission,
                    OwnerId = owner.Id,
                    // car-7 (Mercedes-Benz C-Class) is marked as unavailable in frontend mock data
                    IsActive = carData.Id != "car-7"
                };

                _context.Car.Add(car);
                carDict[carData.Id] = car;
            }
        }

        await _context.SaveChangesAsync();
        return carDict;
    }

    private async Task<Dictionary<string, Booking>> SeedBookingsAsync(Dictionary<string, Car> cars, Dictionary<string, AppUser> users)
    {
        var bookingDict = new Dictionary<string, Booking>();

        var bookingsData = new[]
        {
            new { Id = "booking-1", CarId = "car-1", RenterId = "user-1", StartDate = "2024-12-25", EndDate = "2024-12-28", Status = "Accepted" },
            new { Id = "booking-2", CarId = "car-3", RenterId = "user-1", StartDate = "2024-11-10", EndDate = "2024-11-15", Status = "Completed" },
            new { Id = "booking-3", CarId = "car-2", RenterId = "user-3", StartDate = "2024-12-30", EndDate = "2025-01-05", Status = "Pending" },
            new { Id = "booking-4", CarId = "car-4", RenterId = "user-1", StartDate = "2024-10-15", EndDate = "2024-10-20", Status = "Completed" },
            new { Id = "booking-5", CarId = "car-7", RenterId = "user-3", StartDate = "2024-12-22", EndDate = "2024-12-24", Status = "Rejected" }
        };

        foreach (var bookingData in bookingsData)
        {
            if (cars.TryGetValue(bookingData.CarId, out var car) && users.TryGetValue(bookingData.RenterId, out var renter))
            {
                var startDate = DateOnly.Parse(bookingData.StartDate);
                var endDate = DateOnly.Parse(bookingData.EndDate);
                var days = endDate.DayNumber - startDate.DayNumber + 1;

                var booking = new Booking
                {
                    Id = Guid.NewGuid(),
                    CarId = car.Id,
                    RenterId = renter.Id,
                    StartDate = startDate,
                    EndDate = endDate,
                    TotalPrice = days * car.PricePerDay,
                    Status = bookingData.Status
                };

                _context.Booking.Add(booking);
                bookingDict[bookingData.Id] = booking;
            }
        }

        await _context.SaveChangesAsync();
        return bookingDict;
    }

    private async Task<List<Review>> SeedReviewsAsync(Dictionary<string, Booking> bookings, Dictionary<string, AppUser> users)
    {
        var reviews = new List<Review>();

        var reviewsData = new[]
        {
            new { BookingId = "booking-2", RenterId = "user-1", Rating = 5, Comment = "Amazing car! The Tesla was incredibly smooth and fun to drive. Highly recommend!" },
            new { BookingId = "booking-4", RenterId = "user-1", Rating = 5, Comment = "Luxury at its finest. The BMW X5 was perfect for our family trip." }
        };

        foreach (var reviewData in reviewsData)
        {
            if (bookings.TryGetValue(reviewData.BookingId, out var booking) && users.TryGetValue(reviewData.RenterId, out var author))
            {
                var review = new Review
                {
                    Id = Guid.NewGuid(),
                    BookingId = booking.Id,
                    AuthorId = author.Id,
                    Rating = reviewData.Rating,
                    Comment = reviewData.Comment
                };

                _context.Review.Add(review);
                reviews.Add(review);
            }
        }

        await _context.SaveChangesAsync();
        return reviews;
    }
}

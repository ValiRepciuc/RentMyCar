# Database Seeding Guide

## Overview

The RentMyCar application includes a comprehensive database seeding system that imports mock data from the frontend into PostgreSQL. This ensures a consistent development environment with realistic test data.

## Quick Start

To seed the database with sample data:

```bash
cd backend/Presentation
dotnet run -- --seed
```

## What Gets Seeded

### Users (4 total)
1. **john.doe** (Role: User)
   - Email: john@example.com
   - Password: Password123!
   - City: New York

2. **sarah.johnson** (Role: Owner)
   - Email: sarah@example.com
   - Password: Password123!
   - City: Los Angeles

3. **mike.chen** (Role: Owner)
   - Email: mike@example.com
   - Password: Password123!
   - City: San Francisco

4. **emily.davis** (Role: Owner)
   - Email: emily@example.com
   - Password: Password123!
   - City: Chicago

### Cars (8 total)
- Toyota Camry 2022 - New York - $65/day
- Honda CR-V 2023 - Los Angeles - $75/day
- Tesla Model 3 2023 - San Francisco - $95/day
- BMW X5 2022 - Chicago - $110/day
- Ford Mustang 2023 - Miami - $120/day
- Jeep Wrangler 2022 - Denver - $85/day
- Mercedes-Benz C-Class 2023 - New York - $100/day
- Chevrolet Tahoe 2022 - Austin - $90/day

## Features

### Safe and Repeatable
- The seeder checks if data already exists (by username)
- If users already exist, it skips seeding to prevent duplicates
- Safe to run multiple times

### Image Handling
- Car images use external Pexels URLs
- No local file copying required
- Images are accessible from anywhere

### Proper Relationships
- Cars are assigned to Owner users
- All foreign key relationships are maintained
- Password hashing and salting handled by ASP.NET Identity

## Manual Seeding Steps

If you need to set up everything from scratch:

### 1. Start PostgreSQL

```bash
cd backend
docker compose up -d db
```

Wait for the database to be healthy (about 10 seconds).

### 2. Apply Migrations

```bash
cd backend/Presentation
dotnet ef database update --project ../Domain
```

### 3. Run Seeding

```bash
dotnet run -- --seed
```

### 4. Verify Data

```bash
# Get all cars
curl http://localhost:5039/api/car

# Login with a test user
curl -X POST http://localhost:5039/api/account/login \
  -H "Content-Type: application/json" \
  -c cookies.txt \
  -d '{"userName":"john.doe","password":"Password123!"}'
```

## Architecture

### Clean Architecture Principles

The seeding system follows clean architecture:

- **Domain Layer:** Contains entity definitions (AppUser, Car, Booking)
- **Infrastructure Layer:** Contains DataSeeder service implementation
- **Presentation Layer:** Provides CLI entry point via Program.cs

### Seeder Implementation

Location: `/backend/Infrastructure/Services/DataSeeder.cs`

Key features:
- Implements `IDataSeeder` interface
- Injected via dependency injection
- Uses UserManager for proper password hashing
- Uses EF Core DbContext for car management
- Comprehensive logging

### Registration

The seeder is registered in `/backend/Presentation/Configuration.cs`:

```csharp
services.AddScoped<IDataSeeder, DataSeeder>();
```

### CLI Integration

The seeder is invoked in `/backend/Presentation/Program.cs`:

```csharp
if (args.Contains("--seed"))
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    await seeder.SeedAsync();
    Console.WriteLine("Database seeding completed. Exiting...");
    return;
}
```

## Troubleshooting

### PostgreSQL Not Running

**Error:** `Npgsql.NpgsqlException: Connection refused`

**Solution:**
```bash
cd backend
docker compose up -d db
```

### Database Doesn't Exist

**Error:** `Npgsql.PostgresException: database "RentMyCar" does not exist`

**Solution:**
```bash
cd backend/Presentation
dotnet ef database update --project ../Domain
```

### Seeder Already Ran

**Output:** `Database already seeded. Skipping...`

This is normal and expected. The seeder is idempotent and won't create duplicates.

### Users Already Exist

If you want to re-seed with fresh data, you can:

1. **Drop and recreate the database:**
```bash
cd backend/Presentation
dotnet ef database drop --project ../Domain --force
dotnet ef database update --project ../Domain
dotnet run -- --seed
```

2. **Or manually delete the users:**
```sql
-- Connect to PostgreSQL
DELETE FROM "AspNetUserRoles";
DELETE FROM "AspNetUsers" WHERE "UserName" IN ('john.doe', 'sarah.johnson', 'mike.chen', 'emily.davis');
DELETE FROM "Car";
```

## Extending the Seeder

To add more seed data, edit `/backend/Infrastructure/Services/DataSeeder.cs`:

### Adding More Users

```csharp
var usersData = new[]
{
    // ... existing users ...
    new { MockId = "user-5", UserName = "new.user", Email = "new@example.com", 
          FirstName = "New", LastName = "User", City = "Boston", Role = "User" }
};
```

### Adding More Cars

```csharp
var carsData = new[]
{
    // ... existing cars ...
    new { Brand = "Audi", Model = "A4", Year = 2023, PricePerDay = 85, 
          City = "Seattle", FuelType = "Petrol", Transmission = "Automatic", 
          OwnerId = "user-2", ImageUrl = "https://..." }
};
```

## Frontend Mock Data Source

The seed data is based on `/frontend/src/data/dummyData.ts`:

- Original mock data uses TypeScript interfaces
- Seeder maps frontend types to backend entities
- Property names are normalized (e.g., `make` → `brand`)
- IDs are generated fresh (not using frontend mock IDs)

## Next Steps

After seeding:

1. Start the backend API: `cd backend/Presentation && dotnet run`
2. Start the frontend: `cd frontend && npm run dev`
3. Login with any test user (password: `Password123!`)
4. Browse cars and create bookings

## Summary

The database seeding system provides:
- ✅ Repeatable, safe seeding
- ✅ Real-world test data
- ✅ Proper authentication setup
- ✅ Clean architecture compliance
- ✅ Image handling via URLs
- ✅ Easy CLI access

Run `dotnet run -- --seed` anytime you need fresh test data!

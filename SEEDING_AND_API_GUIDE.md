# RentMyCar Backend - Database Seeding & API Guide

## Database Seeding

### Prerequisites
- PostgreSQL running on localhost:5432
- Database user: `postgres` with password: `postgres`

### Seed Command
To populate the database with mock data from the frontend:

```bash
cd backend/Presentation
dotnet run -- --seed
```

This command will:
1. Create the database if it doesn't exist
2. Apply all migrations
3. Create required roles (User, Owner, Admin, Support)
4. Import mock users, cars, bookings, and reviews
5. Skip seeding if data already exists (idempotent)

### Seeded Data
- **4 Users**: John Doe (User), Sarah Johnson (Owner), Mike Chen (Owner), Emily Davis (Owner)
  - All users have password: `Password123!`
- **8 Cars**: Toyota Camry, Honda CR-V, Tesla Model 3, BMW X5, Ford Mustang, Jeep Wrangler, Mercedes-Benz C-Class, Chevrolet Tahoe
- **5 Bookings**: Various bookings with different statuses
- **2 Reviews**: Reviews for completed bookings

### Verify Seeded Data

```bash
# View all cars
sudo -u postgres psql -d RentMyCar -c "SELECT \"Brand\", \"Model\", \"City\", \"PricePerDay\" FROM \"Car\";"

# View all bookings
sudo -u postgres psql -d RentMyCar -c "SELECT \"StartDate\", \"EndDate\", \"Status\", \"TotalPrice\" FROM \"Booking\";"

# View all users
sudo -u postgres psql -d RentMyCar -c "SELECT \"UserName\", \"Email\", \"City\" FROM \"AspNetUsers\";"
```

## API Testing

### Start the API
```bash
cd backend/Presentation
dotnet run
```

The API will start on `http://localhost:5039`

### Swagger Documentation
Access Swagger UI at: `http://localhost:5039/api/swagger`

## Booking Endpoint - FIXED

### Root Cause of 400 Error
The 400 error was caused by a **JSON naming policy mismatch**:
- Frontend sends: `{"carId": "...", "startDate": "...", "endDate": "..."}`
- Backend expected: `{"CarId": "...", "StartDate": "...", "EndDate": "..."}`

### Solution
Changed `Program.cs` to use camelCase JSON serialization (industry standard):
```csharp
options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
```

### Create Booking - POST /api/booking

#### Request (camelCase JSON)
```bash
curl -X POST http://localhost:5039/api/booking \
  -H "Content-Type: application/json" \
  -H "Cookie: access_token=YOUR_JWT_TOKEN" \
  -d '{
    "carId": "11111111-1111-1111-1111-111111111111",
    "startDate": "2026-02-01",
    "endDate": "2026-02-05"
  }'
```

#### Response (201 Created)
```json
{
  "id": "22222222-2222-2222-2222-222222222222",
  "carId": "11111111-1111-1111-1111-111111111111",
  "carBrand": "Toyota",
  "carModel": "Camry",
  "renterId": "user-id",
  "renterName": "John Doe",
  "startDate": "2026-02-01",
  "endDate": "2026-02-05",
  "totalPrice": 260,
  "status": "Pending"
}
```

### Get All Bookings - GET /api/booking
```bash
curl http://localhost:5039/api/booking \
  -H "Cookie: access_token=YOUR_JWT_TOKEN"
```

### Get Booking by ID - GET /api/booking/{id}
```bash
curl http://localhost:5039/api/booking/22222222-2222-2222-2222-222222222222 \
  -H "Cookie: access_token=YOUR_JWT_TOKEN"
```

### Update Booking - PUT /api/booking/{id}
```bash
curl -X PUT http://localhost:5039/api/booking/22222222-2222-2222-2222-222222222222 \
  -H "Content-Type: application/json" \
  -H "Cookie: access_token=YOUR_JWT_TOKEN" \
  -d '{
    "carId": "11111111-1111-1111-1111-111111111111",
    "startDate": "2026-02-02",
    "endDate": "2026-02-06"
  }'
```

### Accept/Reject Booking - PUT /api/booking/{id}/accept-or-reject
```bash
# Accept booking
curl -X PUT http://localhost:5039/api/booking/22222222-2222-2222-2222-222222222222/accept-or-reject \
  -H "Content-Type: application/json" \
  -H "Cookie: access_token=YOUR_JWT_TOKEN" \
  -d '{
    "accept": true
  }'

# Reject booking
curl -X PUT http://localhost:5039/api/booking/22222222-2222-2222-2222-222222222222/accept-or-reject \
  -H "Content-Type: application/json" \
  -H "Cookie: access_token=YOUR_JWT_TOKEN" \
  -d '{
    "accept": false
  }'
```

### Delete Booking - DELETE /api/booking/{id}
```bash
curl -X DELETE http://localhost:5039/api/booking/22222222-2222-2222-2222-222222222222 \
  -H "Cookie: access_token=YOUR_JWT_TOKEN"
```

## Authentication

### Login
```bash
curl -X POST http://localhost:5039/api/account/login \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "johndoe",
    "password": "Password123!"
  }'
```

The response will include a JWT token in a cookie named `access_token`.

### Test Users (seeded)
| Username | Email | Password | Role |
|----------|-------|----------|------|
| johndoe | john@example.com | Password123! | User |
| sarahjohnson | sarah@example.com | Password123! | Owner |
| mikechen | mike@example.com | Password123! | Owner |
| emilydavis | emily@example.com | Password123! | Owner |

## Car Endpoints

### Get All Cars - GET /api/car
```bash
curl http://localhost:5039/api/car
```

### Get Car by ID - GET /api/car/{id}
```bash
curl http://localhost:5039/api/car/11111111-1111-1111-1111-111111111111
```

### Filter Cars
```bash
# By city
curl "http://localhost:5039/api/car?city=New%20York"

# By price range
curl "http://localhost:5039/api/car?minPrice=50&maxPrice=100"

# By brand
curl "http://localhost:5039/api/car?brand=Toyota"

# Combined filters
curl "http://localhost:5039/api/car?city=New%20York&minPrice=50&maxPrice=100&brand=Toyota"
```

## Images

Car images are served from `/images/` directory:
- `http://localhost:5039/images/toyota.avif`
- `http://localhost:5039/images/honda.jpeg`
- `http://localhost:5039/images/tesla.jpg`
- `http://localhost:5039/images/bmw.webp`
- etc.

## Architecture

### Clean Architecture Principles
- **Domain Layer**: Entities, Repository interfaces (no DTOs)
- **Infrastructure Layer**: DTOs, Services, Data seeding, Repository implementations
- **Presentation Layer**: Controllers, Program.cs, wwwroot for static files

### Key Changes Made
1. **JSON Serialization**: Changed from PascalCase to camelCase (industry standard)
2. **Validation**: Added `[Required]` attributes to CreateBookingDTO
3. **Status Codes**: BookingController now returns 201 Created with Location header
4. **Data Seeding**: Idempotent seeder that imports frontend mock data
5. **Static Files**: wwwroot/images for car images

## Troubleshooting

### Database Connection Issues
If you get connection errors, update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=RentMyCar;Username=postgres;Password=postgres"
  }
}
```

### Reset Database
```bash
# Drop and recreate
sudo -u postgres psql -c "DROP DATABASE IF EXISTS RentMyCar;"
sudo -u postgres psql -c "CREATE DATABASE RentMyCar;"

# Re-seed
cd backend/Presentation
dotnet run -- --seed
```

### Clear Seeded Data
```bash
sudo -u postgres psql -d RentMyCar -c "TRUNCATE TABLE \"Review\", \"Booking\", \"Car\", \"AspNetUserRoles\", \"AspNetUsers\" CASCADE;"
```

## Frontend Integration

The frontend `apiService.ts` already sends camelCase JSON, so no changes are needed on the frontend after the backend fix.

### Example Frontend Booking Creation
```typescript
const booking = await apiService.createBooking({
  carId: car.id,  // Already GUID from GET /api/car
  startDate: "2026-02-01",
  endDate: "2026-02-05"
});
```

## Complete API Specification

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | /api/account/register | Register new user | No |
| POST | /api/account/login | Login | No |
| PUT | /api/account/update | Update user profile | Yes |
| GET | /api/car | Get all cars (with filters) | No |
| GET | /api/car/{id} | Get car by ID | No |
| POST | /api/car | Add new car | Yes (Owner) |
| PUT | /api/car/{id} | Update car | Yes (Owner) |
| DELETE | /api/car/{id} | Delete car | Yes (Owner) |
| GET | /api/booking | Get all bookings | Yes |
| GET | /api/booking/{id} | Get booking by ID | Yes |
| POST | /api/booking | Create booking | Yes (User) |
| PUT | /api/booking/{id} | Update booking | Yes |
| DELETE | /api/booking/{id} | Cancel booking | Yes |
| PUT | /api/booking/{id}/accept-or-reject | Accept/reject booking | Yes (Owner) |
| GET | /get-user-history | Get user's booking history | Yes (User) |
| GET | /get-owner-history | Get owner's booking history | Yes (Owner) |
| GET | /api/review/{bookingId} | Get review by booking | Yes |
| POST | /api/review/{bookingId} | Create review | Yes |

## Environment Variables

No additional environment variables are required. Configuration is in `appsettings.json`:
- ConnectionStrings:DefaultConnection
- JWT:Issuer
- JWT:Audience
- JWT:SigningKey

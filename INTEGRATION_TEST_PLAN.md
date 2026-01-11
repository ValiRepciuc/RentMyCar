# Frontend-Backend Integration Test Plan

## Prerequisites

1. **Backend** must be running on `http://localhost:5039`
2. **Database** must be configured and migrated
3. **Frontend** runs on `http://localhost:5173`

## Starting the Application

### Backend
```bash
cd backend
dotnet restore
dotnet ef database update --project Domain --startup-project Presentation
dotnet run --project Presentation
```

The backend will start on: http://localhost:5039
Swagger UI available at: http://localhost:5039/api/swagger

### Frontend
```bash
cd frontend
npm install
npm run dev
```

The frontend will start on: http://localhost:5173

## Testing the Integration

### 1. Test User Registration
1. Open http://localhost:5173
2. Click "Sign Up" or navigate to register
3. Fill in the form:
   - Name: Test User
   - Email: test@example.com
   - Password: TestPass123!
   - Role: Select either "Rent Cars" or "List My Car"
4. Click "Create Account"
5. You should be automatically logged in and redirected to the cars page

**Expected Backend Call:**
```
POST http://localhost:5039/api/account/register
{
  "Email": "test@example.com",
  "Password": "TestPass123!",
  "UserName": "test",
  "FirstName": "Test",
  "LastName": "User",
  "City": "Unknown",
  "Role": "User" // or "Owner"
}
```

### 2. Test User Login
1. Navigate to login page
2. Enter credentials:
   - Email: test@example.com
   - Password: TestPass123!
3. Click "Sign In"
4. You should be redirected to the cars page

**Expected Backend Call:**
```
POST http://localhost:5039/api/account/login
{
  "UserName": "test@example.com",
  "Password": "TestPass123!"
}
```

### 3. Test Car Listing
1. After logging in, you should see the cars page
2. Cars should load from the backend automatically
3. You can filter by city, price range, etc.

**Expected Backend Call:**
```
GET http://localhost:5039/api/car
```

### 4. Test Car Creation (Owner Role)
1. Login as an owner
2. Navigate to "My Cars" page
3. Click "Add New Car"
4. Fill in the form:
   - Make/Brand: BMW
   - Model: X5
   - Year: 2023
   - Price Per Day: 150
   - City: București
   - Fuel Type: Gasoline
   - Transmission: Automatic
5. Submit the form

**Expected Backend Call:**
```
POST http://localhost:5039/api/car
{
  "Brand": "BMW",
  "Model": "X5",
  "Year": 2023,
  "PricePerDay": 150,
  "City": "București",
  "FuelType": "Gasoline",
  "Transmission": "Automatic"
}
```

### 5. Test Booking Creation (Client Role)
1. Login as a client/user
2. Browse cars and select one
3. Choose start and end dates
4. Click "Book Now"
5. Booking should be created with "Pending" status

**Expected Backend Call:**
```
POST http://localhost:5039/api/booking
{
  "CarId": "guid-of-car",
  "StartDate": "2024-01-15",
  "EndDate": "2024-01-20"
}
```

### 6. Test Booking Management (Owner Role)
1. Login as an owner
2. Navigate to "Manage Bookings"
3. You should see pending bookings for your cars
4. Click "Accept" or "Reject" on a booking

**Expected Backend Call:**
```
PUT http://localhost:5039/api/booking/{bookingId}/accept-or-reject
{
  "Accept": true // or false
}
```

## Troubleshooting

### CORS Errors
If you see CORS errors in the browser console:
1. Check that backend is running on port 5039
2. Verify CORS configuration in `backend/Presentation/Program.cs` allows origin `http://localhost:5173`

### 401 Unauthorized Errors
If API calls return 401:
1. Check that JWT token is being saved in cookies
2. Open browser DevTools > Application > Cookies
3. Look for `access_token` cookie
4. If missing, there's an issue with login/register

### Connection Refused
If you get "Connection refused" errors:
1. Ensure backend is running: `dotnet run --project backend/Presentation`
2. Check that it's listening on port 5039
3. Verify no firewall is blocking the connection

### Data Not Loading
If cars or bookings don't appear:
1. Check browser console for error messages
2. Check backend logs for errors
3. Verify database has data
4. Check that API endpoints are returning data (test in Swagger)

## Data Mapping

The integration includes automatic mapping between backend DTOs and frontend types:

### User Mapping
- Backend: `FirstName`, `LastName` → Frontend: `name`
- Backend: `Role` ("User"/"Owner") → Frontend: `role` ("client"/"owner")
- Backend: `Email` → Frontend: `id` and `email`

### Car Mapping
- Backend: `Brand` → Frontend: `make`
- Backend: `Id` (Guid) → Frontend: `id` (string)
- Backend: `PricePerDay` → Frontend: `pricePerDay`
- Backend: `IsActive` → Frontend: `available`

### Booking Mapping
- Backend: `RenterId` → Frontend: `clientId`
- Backend: `Status` (Pascal case) → Frontend: `status` (lowercase)
- Backend: `StartDate`/`EndDate` (DateOnly) → Frontend: dates as strings

## Notes

- All functions in AppContext are now async and return Promises
- The app falls back to dummy data if API calls fail
- JWT tokens are automatically included in all authenticated requests via cookies
- The frontend automatically loads cars on mount and after login

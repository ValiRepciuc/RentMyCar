# API Request/Response Examples

## Booking Endpoints

### Create Booking

**Endpoint**: `POST /api/booking`

**Request Headers**:
```
Content-Type: application/json
Authorization: Bearer YOUR_JWT_TOKEN
```

**Request Body** (camelCase):
```json
{
  "carId": "ecaf0596-d320-4089-92cf-01db1a1099b0",
  "startDate": "2026-03-01",
  "endDate": "2026-03-05"
}
```

**Response** (201 Created):
```json
{
  "id": "166a6316-6075-462f-86bb-a557b3f0a508",
  "carId": "ecaf0596-d320-4089-92cf-01db1a1099b0",
  "carBrand": "Toyota",
  "carModel": "Camry",
  "renterId": "eaf5b151-7ffa-4d83-a058-7262fb7bf611",
  "renterName": "John Doe",
  "startDate": "2026-03-01",
  "endDate": "2026-03-05",
  "totalPrice": 325,
  "status": "Pending"
}
```

**Error Response** (400 Bad Request):
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "traceId": "00-...",
  "errors": {
    "CarId": [
      "Car ID is required"
    ]
  }
}
```

### Full Working Example (bash)
```bash
#!/bin/bash

# 1. Login
TOKEN=$(curl -s -X POST http://localhost:5039/api/account/login \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "johndoe",
    "password": "Password123!"
  }' | jq -r '.token')

echo "Token obtained: ${TOKEN:0:50}..."

# 2. Get available cars
CAR_ID=$(curl -s http://localhost:5039/api/car | jq -r '.[0].id')
echo "Using car ID: $CAR_ID"

# 3. Create booking
BOOKING_RESPONSE=$(curl -s -X POST http://localhost:5039/api/booking \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d "{
    \"carId\": \"$CAR_ID\",
    \"startDate\": \"2026-03-01\",
    \"endDate\": \"2026-03-05\"
  }")

echo "Booking created:"
echo $BOOKING_RESPONSE | jq .

# 4. Get booking details
BOOKING_ID=$(echo $BOOKING_RESPONSE | jq -r '.id')
curl -s http://localhost:5039/api/booking/$BOOKING_ID \
  -H "Authorization: Bearer $TOKEN" | jq .
```

### curl Commands

#### Create Booking (Standalone)
```bash
curl -X POST http://localhost:5039/api/booking \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9..." \
  -d '{
    "carId": "ecaf0596-d320-4089-92cf-01db1a1099b0",
    "startDate": "2026-03-01",
    "endDate": "2026-03-05"
  }'
```

#### Get All Bookings
```bash
curl http://localhost:5039/api/booking \
  -H "Authorization: Bearer YOUR_TOKEN"
```

#### Get Booking by ID
```bash
curl http://localhost:5039/api/booking/166a6316-6075-462f-86bb-a557b3f0a508 \
  -H "Authorization: Bearer YOUR_TOKEN"
```

#### Update Booking
```bash
curl -X PUT http://localhost:5039/api/booking/166a6316-6075-462f-86bb-a557b3f0a508 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "carId": "ecaf0596-d320-4089-92cf-01db1a1099b0",
    "startDate": "2026-03-02",
    "endDate": "2026-03-06"
  }'
```

#### Delete Booking
```bash
curl -X DELETE http://localhost:5039/api/booking/166a6316-6075-462f-86bb-a557b3f0a508 \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### JavaScript/TypeScript Example

```typescript
// Frontend usage (already working)
const apiService = {
  async createBooking(data: {
    carId: string;
    startDate: string;
    endDate: string;
  }): Promise<BookingDTO> {
    const response = await fetch('http://localhost:5039/api/booking', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify({
        carId: data.carId,        // camelCase ✅
        startDate: data.startDate, // camelCase ✅
        endDate: data.endDate      // camelCase ✅
      })
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to create booking');
    }
    
    return response.json();
  }
};

// Usage
const booking = await apiService.createBooking({
  carId: "ecaf0596-d320-4089-92cf-01db1a1099b0",
  startDate: "2026-03-01",
  endDate: "2026-03-05"
});

console.log(booking);
// {
//   id: "166a6316-6075-462f-86bb-a557b3f0a508",
//   carId: "ecaf0596-d320-4089-92cf-01db1a1099b0",
//   carBrand: "Toyota",
//   carModel: "Camry",
//   ...
// }
```

### Python Example

```python
import requests
import json

# Login
login_response = requests.post(
    'http://localhost:5039/api/account/login',
    headers={'Content-Type': 'application/json'},
    json={
        'userName': 'johndoe',
        'password': 'Password123!'
    }
)

token = login_response.json()['token']

# Create booking
booking_response = requests.post(
    'http://localhost:5039/api/booking',
    headers={
        'Content-Type': 'application/json',
        'Authorization': f'Bearer {token}'
    },
    json={
        'carId': 'ecaf0596-d320-4089-92cf-01db1a1099b0',
        'startDate': '2026-03-01',
        'endDate': '2026-03-05'
    }
)

booking = booking_response.json()
print(json.dumps(booking, indent=2))
```

### C# Example

```csharp
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

// Login
var loginRequest = new
{
    userName = "johndoe",
    password = "Password123!"
};

var loginResponse = await httpClient.PostAsJsonAsync(
    "http://localhost:5039/api/account/login",
    loginRequest
);

var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
var token = loginResult.Token;

// Create booking
var bookingRequest = new
{
    carId = "ecaf0596-d320-4089-92cf-01db1a1099b0",
    startDate = "2026-03-01",
    endDate = "2026-03-05"
};

httpClient.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", token);

var bookingResponse = await httpClient.PostAsJsonAsync(
    "http://localhost:5039/api/booking",
    bookingRequest,
    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
);

var booking = await bookingResponse.Content.ReadFromJsonAsync<BookingDTO>();
Console.WriteLine(JsonSerializer.Serialize(booking, new JsonSerializerOptions 
{ 
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
}));
```

## Other Endpoints

### Authentication

#### Register
```bash
curl -X POST http://localhost:5039/api/account/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "newuser@example.com",
    "userName": "newuser",
    "firstName": "New",
    "lastName": "User",
    "city": "Boston",
    "role": "user",
    "password": "Password123!"
  }'
```

#### Login
```bash
curl -X POST http://localhost:5039/api/account/login \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "johndoe",
    "password": "Password123!"
  }'
```

### Cars

#### Get All Cars
```bash
curl http://localhost:5039/api/car
```

#### Get Car by ID
```bash
curl http://localhost:5039/api/car/ecaf0596-d320-4089-92cf-01db1a1099b0
```

#### Filter Cars by City
```bash
curl "http://localhost:5039/api/car?city=New%20York"
```

#### Filter Cars by Price Range
```bash
curl "http://localhost:5039/api/car?minPrice=50&maxPrice=100"
```

#### Add Car (Owner only)
```bash
curl -X POST http://localhost:5039/api/car \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_OWNER_TOKEN" \
  -d '{
    "brand": "Audi",
    "model": "A4",
    "year": 2023,
    "pricePerDay": 85,
    "city": "Seattle",
    "fuelType": "Hybrid",
    "transmission": "Automatic"
  }'
```

### Reviews

#### Get Review by Booking
```bash
curl http://localhost:5039/api/review/{bookingId} \
  -H "Authorization: Bearer YOUR_TOKEN"
```

#### Create Review
```bash
curl -X POST http://localhost:5039/api/review/{bookingId} \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "rating": 5,
    "comment": "Great car! Highly recommend."
  }'
```

## Expected Data Types

### CreateBookingDTO
```json
{
  "carId": "string (GUID format)",
  "startDate": "string (ISO 8601 date: YYYY-MM-DD)",
  "endDate": "string (ISO 8601 date: YYYY-MM-DD)"
}
```

### BookingDTO (Response)
```json
{
  "id": "string (GUID)",
  "carId": "string (GUID)",
  "carBrand": "string",
  "carModel": "string",
  "renterId": "string (GUID)",
  "renterName": "string",
  "startDate": "string (YYYY-MM-DD)",
  "endDate": "string (YYYY-MM-DD)",
  "totalPrice": "number (integer)",
  "status": "string (Pending|Accepted|Rejected|Completed)"
}
```

### CarDTO (Response)
```json
{
  "id": "string (GUID)",
  "brand": "string",
  "model": "string",
  "year": "number (integer)",
  "pricePerDay": "number (integer)",
  "city": "string",
  "fuelType": "string (Gasoline|Diesel|Hybrid|Electric)",
  "transmission": "string (Manual|Automatic)",
  "isActive": "boolean",
  "ownerId": "string (GUID)",
  "ownerName": "string"
}
```

## Common Errors

### 400 Bad Request - Missing Required Field
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "CarId": ["Car ID is required"]
  }
}
```

### 401 Unauthorized - Missing/Invalid Token
```json
["Error! User is not logged in."]
```

### 500 Internal Server Error - Business Logic
```json
["This car is already booked in the selected period."]
```

### 404 Not Found
```json
["Booking not found"]
```

## Testing Checklist

- [x] Login with valid credentials
- [x] Get JWT token from response
- [x] Use token in Authorization header
- [x] Create booking with camelCase JSON
- [x] Verify 201 Created response
- [x] Verify response contains all fields
- [x] Verify dates are formatted correctly
- [x] Verify totalPrice is calculated
- [x] Get booking by ID
- [x] Get all bookings
- [x] Update booking (if allowed)
- [x] Delete booking (if allowed)
- [x] Handle validation errors gracefully
- [x] Handle business logic errors
- [x] Handle unauthorized access

## Notes

1. **Case Sensitivity**: The API accepts both camelCase and PascalCase for input, but always returns camelCase
2. **Date Format**: Use ISO 8601 date format (YYYY-MM-DD) for dates
3. **GUID Format**: All IDs are GUIDs in the format: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
4. **Authentication**: JWT tokens are returned in the response body (not cookies)
5. **Authorization**: Use `Authorization: Bearer {token}` header for authenticated endpoints
6. **Content-Type**: Always use `Content-Type: application/json` for POST/PUT requests

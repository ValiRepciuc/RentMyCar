# RentMyCar API Documentation

## Overview

The RentMyCar API uses **camelCase** for JSON properties (both request and response). All endpoints require `Content-Type: application/json`.

## Authentication

The API uses JWT tokens stored in HTTP-only cookies. After login/register, the token is automatically set in a cookie named `access_token`.

## Base URL

```
http://localhost:5039/api
```

## Endpoints

### 1. Register a User

**POST** `/account/register`

**Request Body:**
```json
{
  "userName": "john.doe",
  "password": "Password123!",
  "email": "john@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "city": "New York",
  "role": "User"
}
```

**Response:**
```json
{
  "email": "john@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "userName": "john.doe",
  "city": "New York",
  "role": "User",
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9..."
}
```

**cURL Example:**
```bash
curl -X POST http://localhost:5039/api/account/register \
  -H "Content-Type: application/json" \
  -c cookies.txt \
  -d '{
    "userName": "john.doe",
    "password": "Password123!",
    "email": "john@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "city": "New York",
    "role": "User"
  }'
```

---

### 2. Login

**POST** `/account/login`

**Request Body:**
```json
{
  "userName": "john.doe",
  "password": "Password123!"
}
```

**Response:**
```json
{
  "email": "john@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "userName": "john.doe",
  "city": "New York",
  "role": "User",
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9..."
}
```

**cURL Example:**
```bash
curl -X POST http://localhost:5039/api/account/login \
  -H "Content-Type: application/json" \
  -c cookies.txt \
  -d '{
    "userName": "john.doe",
    "password": "Password123!"
  }'
```

---

### 3. Get All Cars

**GET** `/car`

**No authentication required**

**Response:**
```json
[
  {
    "id": "f01a5149-0d2b-44c9-beb7-8f6715a32f68",
    "brand": "Honda",
    "model": "CR-V",
    "year": 2023,
    "pricePerDay": 75,
    "city": "Los Angeles",
    "fuelType": "Hybrid",
    "transmission": "Automatic",
    "isActive": true,
    "ownerId": "164cb021-7db1-471d-a3f1-05e523b3b78e",
    "ownerName": "Sarah Johnson"
  }
]
```

**cURL Example:**
```bash
curl http://localhost:5039/api/car
```

---

### 4. Create a Booking

**POST** `/booking`

**Requires authentication** (User role)

**Request Body:**
```json
{
  "carId": "f01a5149-0d2b-44c9-beb7-8f6715a32f68",
  "startDate": "2025-06-15",
  "endDate": "2025-06-20"
}
```

**Response:**
```json
{
  "id": "9a96a037-2483-4144-9a8c-423aaacddcb9",
  "carId": "f01a5149-0d2b-44c9-beb7-8f6715a32f68",
  "carBrand": "Honda",
  "carModel": "CR-V",
  "renterId": "c6b65863-5c0d-44f4-a0b1-da7dfcb535da",
  "renterName": "Test User",
  "startDate": "2025-06-15",
  "endDate": "2025-06-20",
  "totalPrice": 450,
  "status": "Pending"
}
```

**cURL Example:**
```bash
# First login to get the cookie
curl -X POST http://localhost:5039/api/account/login \
  -H "Content-Type: application/json" \
  -c cookies.txt \
  -d '{"userName":"john.doe","password":"Password123!"}'

# Then create a booking
curl -X POST http://localhost:5039/api/booking \
  -H "Content-Type: application/json" \
  -b cookies.txt \
  -d '{
    "carId": "f01a5149-0d2b-44c9-beb7-8f6715a32f68",
    "startDate": "2025-06-15",
    "endDate": "2025-06-20"
  }'
```

---

### 5. Get All Bookings

**GET** `/booking`

**Requires authentication**

**Response:**
```json
[
  {
    "id": "9a96a037-2483-4144-9a8c-423aaacddcb9",
    "carId": "f01a5149-0d2b-44c9-beb7-8f6715a32f68",
    "carBrand": "Honda",
    "carModel": "CR-V",
    "renterId": "c6b65863-5c0d-44f4-a0b1-da7dfcb535da",
    "renterName": "Test User",
    "startDate": "2025-06-15",
    "endDate": "2025-06-20",
    "totalPrice": 450,
    "status": "Pending"
  }
]
```

**cURL Example:**
```bash
curl -X GET http://localhost:5039/api/booking \
  -b cookies.txt
```

---

## Postman Collection

### Setting Up Postman

1. **Create a new collection** named "RentMyCar API"
2. **Add a variable** named `baseUrl` with value `http://localhost:5039/api`
3. **Enable automatic cookie management** in Postman settings

### Request Examples

#### 1. Register

- **Method:** POST
- **URL:** `{{baseUrl}}/account/register`
- **Headers:**
  - `Content-Type: application/json`
- **Body (raw JSON):**
```json
{
  "userName": "john.doe",
  "password": "Password123!",
  "email": "john@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "city": "New York",
  "role": "User"
}
```

#### 2. Login

- **Method:** POST
- **URL:** `{{baseUrl}}/account/login`
- **Headers:**
  - `Content-Type: application/json`
- **Body (raw JSON):**
```json
{
  "userName": "john.doe",
  "password": "Password123!"
}
```
- **Tests tab:** Add this script to save the token:
```javascript
pm.test("Login successful", function () {
    pm.response.to.have.status(200);
    var jsonData = pm.response.json();
    pm.environment.set("token", jsonData.token);
});
```

#### 3. Get All Cars

- **Method:** GET
- **URL:** `{{baseUrl}}/car`
- No authentication required

#### 4. Create Booking

- **Method:** POST
- **URL:** `{{baseUrl}}/booking`
- **Headers:**
  - `Content-Type: application/json`
- **Body (raw JSON):**
```json
{
  "carId": "f01a5149-0d2b-44c9-beb7-8f6715a32f68",
  "startDate": "2025-06-15",
  "endDate": "2025-06-20"
}
```
- **Note:** Make sure to login first so the cookie is set

---

## Database Seeding

To populate the database with sample data from frontend mock:

```bash
cd backend/Presentation
dotnet run -- --seed
```

This will:
- Create 4 users (1 User role, 3 Owner roles)
- Create 8 cars
- All with password: `Password123!`

**Test Users:**
- Username: `john.doe` (Role: User)
- Username: `sarah.johnson` (Role: Owner)
- Username: `mike.chen` (Role: Owner)
- Username: `emily.davis` (Role: Owner)

---

## Error Responses

All errors return appropriate HTTP status codes with JSON error messages:

### 400 Bad Request
```json
{
  "errors": {
    "carId": ["CarId is required"],
    "startDate": ["StartDate is required"]
  }
}
```

### 401 Unauthorized
```json
["Invalid username or password."]
```

### 500 Internal Server Error
```json
["Error! User is not logged in."]
```

---

## Important Notes

1. **JSON Naming:** All JSON properties use **camelCase** (e.g., `carId`, `startDate`, not `CarId`, `StartDate`)
2. **Authentication:** JWT tokens are stored in HTTP-only cookies automatically after login
3. **GUID Format:** All IDs are GUIDs in standard format: `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx`
4. **Date Format:** Dates use ISO 8601 format: `YYYY-MM-DD`
5. **CORS:** The API allows requests from `http://localhost:5173` (frontend)

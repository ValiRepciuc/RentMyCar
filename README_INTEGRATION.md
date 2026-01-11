# RentMyCar - Frontend-Backend Integration Guide

This guide provides complete instructions for setting up and running the RentMyCar application with full integration between the frontend and backend.

## Table of Contents
- [Prerequisites](#prerequisites)
- [First-Time Setup](#first-time-setup)
- [Running the Application](#running-the-application)
- [Database Management](#database-management)
- [Environment Configuration](#environment-configuration)
- [Testing the Integration](#testing-the-integration)
- [Troubleshooting](#troubleshooting)

## Prerequisites

- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js 18+** and npm - [Download](https://nodejs.org/)
- **Docker** and Docker Compose - [Download](https://www.docker.com/products/docker-desktop)
- **Git** - [Download](https://git-scm.com/)

## First-Time Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd RentMyCar
```

### 2. Start the Database

```bash
# Start PostgreSQL and pgAdmin containers
docker compose up -d

# Verify containers are running
docker compose ps
```

The database will be available at:
- **PostgreSQL**: `localhost:5439`
- **pgAdmin**: `http://localhost:5050` (admin@rentmycar.com / admin)

### 3. Setup Backend

```bash
cd backend

# Restore NuGet packages
dotnet restore

# Apply database migrations
dotnet ef database update --project Domain --startup-project Presentation

# Seed the database with initial data (cars and users)
dotnet run --project Presentation -- --seed
```

**Default Seeded Users:**
- `sarah@example.com` / `Password123!` (Owner)
- `mike@example.com` / `Password123!` (Owner)
- `emily@example.com` / `Password123!` (Owner)

### 4. Setup Frontend

```bash
cd ../frontend

# Install dependencies
npm install

# Create .env file (optional - defaults are already set)
echo "VITE_API_URL=http://localhost:5039" > .env
```

## Running the Application

### Start All Services

**Terminal 1 - Database (if not already running):**
```bash
docker compose up -d
```

**Terminal 2 - Backend:**
```bash
cd backend
dotnet run --project Presentation
```
Backend will start at: `http://localhost:5039`

**Terminal 3 - Frontend:**
```bash
cd frontend
npm run dev
```
Frontend will start at: `http://localhost:5173`

### Access the Application

Open your browser and navigate to: `http://localhost:5173`

## Database Management

### View Database with pgAdmin

1. Open pgAdmin at `http://localhost:5050`
2. Login with: `admin@rentmycar.com` / `admin`
3. Add a new server:
   - **Name**: RentMyCar Local
   - **Host**: `postgres` (or `localhost` if connecting from host machine)
   - **Port**: `5432` (internal) or `5439` (from host)
   - **Username**: `rentmycar`
   - **Password**: `rentmycar`
   - **Database**: `RentMyCar`

### Reset Database

To completely reset the database and start fresh:

```bash
# Stop and remove containers
docker compose down -v

# Start fresh
docker compose up -d

# Apply migrations
cd backend
dotnet ef database update --project Domain --startup-project Presentation

# Re-seed data
dotnet run --project Presentation -- --seed
```

### Create New Migration

When you modify entity models:

```bash
cd backend
dotnet ef migrations add YourMigrationName --project Domain --startup-project Presentation
dotnet ef database update --project Domain --startup-project Presentation
```

### Reseed Data

To reseed the database without recreating it:

```bash
# Manually delete existing cars (using pgAdmin or psql)
# Or drop and recreate the database

cd backend
dotnet run --project Presentation -- --seed
```

## Environment Configuration

### Backend Configuration

**Location**: `/backend/Presentation/appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5439;Database=RentMyCar;Username=rentmycar;Password=rentmycar;Include Error Detail=true"
  },
  "JWT": {
    "Issuer": "http://localhost:5039",
    "Audience": "http://localhost:5039",
    "SigningKey": "ThisIsAVerySecureKeyForDevelopmentPurposesOnly123456789"
  }
}
```

### Frontend Configuration

**Location**: `/frontend/.env`

```env
VITE_API_URL=http://localhost:5039
```

### Docker Compose Configuration

**Location**: `/.env`

```env
POSTGRES_USER=rentmycar
POSTGRES_PASSWORD=rentmycar
POSTGRES_DB=RentMyCar
POSTGRES_HOST_PORT=5439
PGADMIN_EMAIL=admin@rentmycar.com
PGADMIN_PASSWORD=admin
```

## Testing the Integration

### 1. Verify Backend API

Test the cars endpoint:
```bash
curl http://localhost:5039/api/car
```

Test static image serving:
```bash
curl -I http://localhost:5039/images/bmw.webp
```

### 2. Test Frontend Features

1. **Car Listing**
   - Open `http://localhost:5173`
   - Verify all cars are displayed with images
   - Test filtering by city, brand, price range

2. **Car Details**
   - Click on any car
   - Verify all details are shown correctly
   - Verify images carousel works

3. **Authentication**
   - Click "Login"
   - Use: `sarah@example.com` / `Password123!`
   - Verify successful login

4. **Create Booking**
   - Login as a user
   - Select a car
   - Choose dates and create a booking
   - Verify booking is created

### 3. Test CORS

If you encounter CORS errors, verify:
- Backend CORS policy includes `http://localhost:5173`
- Frontend uses `credentials: 'include'` in API calls
- Cookies are being set correctly

## Troubleshooting

### Port Already in Use

If port 5439 (PostgreSQL) or 5039 (Backend) or 5173 (Frontend) is already in use:

**Change PostgreSQL port**:
1. Edit `/.env` - change `POSTGRES_HOST_PORT=5439` to another port
2. Edit `/backend/Presentation/appsettings.Development.json` - update connection string port
3. Restart: `docker compose down && docker compose up -d`

**Change Backend port**:
1. Edit `/backend/Presentation/Properties/launchSettings.json`
2. Update frontend API URL in `/frontend/.env`

**Change Frontend port**:
1. Update CORS policy in `/backend/Presentation/Program.cs`

### Database Connection Fails

```bash
# Check if PostgreSQL container is running
docker compose ps

# Check PostgreSQL logs
docker compose logs postgres

# Restart containers
docker compose restart
```

### Frontend Can't Connect to Backend

1. Verify backend is running: `curl http://localhost:5039/api/car`
2. Check CORS configuration in `Program.cs`
3. Verify `.env` file exists in frontend with correct API URL
4. Clear browser cache and cookies

### Entity Framework Errors

```bash
# Remove existing migrations (if needed)
cd backend/Domain/Migrations
# Delete migration files except DatabaseContextModelSnapshot.cs

# Create fresh migration
cd ../..
dotnet ef migrations add InitialCreate --project Domain --startup-project Presentation
dotnet ef database update --project Domain --startup-project Presentation
```

### Images Not Loading

1. Verify images exist in `/backend/Presentation/wwwroot/images/`
2. Check static file middleware is configured in `Program.cs`
3. Test direct image URL: `http://localhost:5039/images/bmw.webp`
4. Check browser console for errors

## Project Structure

```
RentMyCar/
├── backend/
│   ├── Domain/               # Entity models, DbContext, Repositories
│   ├── Infrastructure/       # DTOs, Services, Mappers
│   └── Presentation/         # API Controllers, Configuration
│       └── wwwroot/
│           └── images/       # Static car images
├── frontend/
│   └── src/
│       ├── config/          # API configuration
│       ├── services/        # API service layer
│       ├── context/         # React Context (AppContext)
│       └── pages/           # React pages/components
├── docker-compose.yml       # Database orchestration
└── .env                    # Docker Compose variables
```

## Key Integration Points

### Backend → Frontend Data Flow

1. **Car Listing**: `GET /api/car` → `apiService.getCars()` → `AppContext.fetchCars()`
2. **Car Details**: `GET /api/car/{id}` → `apiService.getCarById()`
3. **Authentication**: `POST /api/account/login` → `apiService.login()` → `AppContext.login()`
4. **Create Booking**: `POST /api/booking` → `apiService.createBooking()` → `AppContext.createBooking()`

### Data Mapping

Frontend `Car` type → Backend `CarDTO`:
- `make` → `Brand`
- `image` → `ImageUrl`
- `images` → `ImageUrls`
- `available` → `IsActive`
- `fuelType` → `FuelType` (lowercase → capitalized)

### Authentication Flow

1. User logs in via frontend
2. Backend validates credentials and returns JWT token
3. Token is stored in `access_token` cookie
4. Frontend includes cookie in subsequent requests via `credentials: 'include'`
5. Backend validates token from cookie on protected endpoints

## Common Development Commands

```bash
# Backend
cd backend
dotnet build                                      # Build solution
dotnet run --project Presentation                 # Run backend
dotnet ef migrations add MigrationName           # Create migration
dotnet ef database update                        # Apply migrations
dotnet run --project Presentation -- --seed      # Seed database

# Frontend
cd frontend
npm install                                      # Install dependencies
npm run dev                                      # Run dev server
npm run build                                    # Build for production
npm run preview                                  # Preview production build

# Docker
docker compose up -d                             # Start containers
docker compose down                              # Stop containers
docker compose down -v                           # Stop and remove volumes
docker compose ps                                # List containers
docker compose logs postgres                     # View logs
```

## Support

For issues or questions, please refer to the integration guide document: `GHID_INTEGRARE_FRONTEND_BACKEND.md`

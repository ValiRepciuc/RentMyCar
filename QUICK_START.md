# RentMyCar - Quick Start Guide

## 🚀 Quick Start (First Time)

```bash
# 1. Start database
docker compose up -d

# 2. Setup and run backend
cd backend
dotnet restore
dotnet ef database update --project Domain --startup-project Presentation
dotnet run --project Presentation -- --seed

# 3. In a new terminal, setup and run frontend
cd frontend
npm install
npm run dev
```

## 🔄 Daily Development

```bash
# Terminal 1: Database (if not running)
docker compose up -d

# Terminal 2: Backend
cd backend
dotnet run --project Presentation

# Terminal 3: Frontend
cd frontend
npm run dev
```

## 📍 Access Points

- **Frontend**: http://localhost:5173
- **Backend API**: http://localhost:5039
- **Backend Swagger**: http://localhost:5039/api/swagger
- **pgAdmin**: http://localhost:5050

## 🔐 Test Credentials

- `sarah@example.com` / `Password123!` (Owner)
- `mike@example.com` / `Password123!` (Owner)
- `emily@example.com` / `Password123!` (Owner)

## 🗄️ Database Reset

```bash
docker compose down -v
docker compose up -d
cd backend
dotnet ef database update --project Domain --startup-project Presentation
dotnet run --project Presentation -- --seed
```

## ✅ Verification Checklist

- [ ] Database running: `docker compose ps`
- [ ] Backend API works: `curl http://localhost:5039/api/car`
- [ ] Images load: `curl -I http://localhost:5039/images/bmw.webp`
- [ ] Frontend loads: Open http://localhost:5173
- [ ] Cars display with images
- [ ] Login works with test credentials
- [ ] Booking creation works

## 📝 Changes Made

### Backend Changes
1. **docker-compose.yml** - PostgreSQL + pgAdmin containers
2. **appsettings.json** - Updated connection string to port 5439
3. **Car entity** - Added Description, Features, ImageUrl, ImageUrls, Seats, Rating, ReviewCount
4. **CarDTO** - Added new fields to match frontend
5. **CarMapper** - JSON serialization for arrays
6. **Migration** - AddCarFrontendFields migration
7. **Program.cs** - Static files middleware + seed command
8. **SeedData.cs** - Import mockdata from frontend
9. **wwwroot/images/** - Copied frontend images

### Frontend Changes
1. **config/api.ts** - API configuration and endpoints
2. **services/apiService.ts** - API service layer with all endpoints
3. **context/AppContext.tsx** - Replaced mockdata with API calls
4. **pages/Login.tsx** - Made login async
5. **pages/Register.tsx** - Made register async
6. **.env** - API URL configuration

### Configuration Files
1. **.env** (root) - Docker Compose variables
2. **frontend/.env** - Frontend API URL
3. **backend/Presentation/appsettings.Development.json** - Development config

## 🐛 Quick Troubleshooting

**"Port already in use"**
- Change ports in .env and appsettings.json

**"Database connection failed"**
- Run: `docker compose restart postgres`

**"Cars not loading in frontend"**
- Check backend is running: `curl http://localhost:5039/api/car`
- Check browser console for errors
- Verify CORS settings in Program.cs

**"Images not showing"**
- Verify: `curl -I http://localhost:5039/images/bmw.webp`
- Check browser network tab
- Ensure wwwroot/images/ has files

## 📚 Full Documentation

See `README_INTEGRATION.md` for complete documentation.

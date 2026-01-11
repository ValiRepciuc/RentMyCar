# Ghid de Integrare Frontend-Backend pentru RentMyCar

Acest document explică unde trebuie să te uiți în cod pentru a lega frontend-ul de backend în aplicația RentMyCar.

## Structura Proiectului

```
RentMyCar/
├── backend/           # Backend .NET 8
│   ├── Domain/        # Entități și baza de date
│   ├── Infrastructure/# DTOs și Services
│   └── Presentation/  # Controllers și configurare API
└── frontend/          # Frontend React + TypeScript
    └── src/
        ├── pages/     # Pagini UI
        ├── context/   # State management (AppContext.tsx)
        ├── types/     # TypeScript types
        └── components/# Componente reutilizabile
```

---

## 1. BACKEND - Unde să te uiți

### 1.1 Controllers (API Endpoints)

**Locație:** `/backend/Presentation/Controllers/`

Aici găsești toate endpoint-urile disponibile:

#### **AccountController.cs** - Autentificare și cont utilizator
- `POST /api/account/register` - Înregistrare utilizator nou
- `POST /api/account/login` - Login
- `PUT /api/account/update` - Actualizare profil utilizator (necesită autentificare)

#### **CarController.cs** - Gestionare mașini
- `GET /api/car` - Lista toate mașinile (cu filtre opționale: city, minPrice, maxPrice, brand, model)
- `GET /api/car/{id}` - Detalii despre o mașină specifică
- `POST /api/car` - Adaugă mașină nouă
- `PUT /api/car/{id}` - Actualizează o mașină
- `DELETE /api/car/{id}` - Șterge o mașină

#### **BookingController.cs** - Gestionare rezervări
- `GET /api/booking` - Lista toate rezervările
- `GET /api/booking/{id}` - Detalii despre o rezervare
- `POST /api/booking` - Creează rezervare nouă
- `PUT /api/booking/{id}` - Actualizează o rezervare
- `DELETE /api/booking/{id}` - Șterge o rezervare
- `PUT /api/booking/{bookingId}/accept-or-reject` - Acceptă/Refuză o rezervare
- `GET /get-user-history` - Istoric rezervări pentru utilizator (Role: User)
- `GET /get-owner-history` - Istoric rezervări pentru proprietar (Role: Owner)

#### **ReviewController.cs** - Gestionare recenzii
- `GET /api/review/{bookingId}` - Obține recenzia pentru o rezervare
- `POST /api/review/{bookingId}` - Creează recenzie pentru o rezervare

### 1.2 DTOs (Data Transfer Objects)

**Locație:** `/backend/Infrastructure/DTOs/`

DTOs definesc structura datelor trimise/primite la/din API:

#### Account DTOs
- **RegisterDTO** - Pentru înregistrare:
  ```csharp
  {
    "Email": "user@example.com",
    "Password": "Password123!",
    "UserName": "username",
    "FirstName": "John",
    "LastName": "Doe",
    "City": "București",
    "Role": "User" // sau "Owner"
  }
  ```

- **LoginDTO** - Pentru login:
  ```csharp
  {
    "UserName": "username",
    "Password": "Password123!"
  }
  ```

- **NewUserDTO** - Răspuns după login/register (include Token JWT)

#### Car DTOs
- **CarDTO** - Structura unui obiect Car:
  ```csharp
  {
    "Id": "guid",
    "Brand": "BMW",
    "Model": "X5",
    "Year": 2023,
    "PricePerDay": 150,
    "City": "București",
    "FuelType": "Petrol",
    "Transmission": "Automatic",
    "IsActive": true,
    "OwnerId": "string",
    "OwnerName": "string"
  }
  ```

- **CreateCarDTO** - Pentru a adăuga o mașină nouă
- **UpdateCarDTO** - Pentru a actualiza o mașină

#### Booking DTOs
- **BookingDTO** - Structura unei rezervări:
  ```csharp
  {
    "Id": "guid",
    "CarId": "guid",
    "CarBrand": "BMW",
    "CarModel": "X5",
    "RenterId": "string",
    "RenterName": "John Doe",
    "StartDate": "2024-01-15",
    "EndDate": "2024-01-20",
    "TotalPrice": 750,
    "Status": "Pending" // Pending, Accepted, Rejected, Completed, Cancelled
  }
  ```

- **CreateBookingDTO** - Pentru a crea o rezervare nouă:
  ```csharp
  {
    "CarId": "guid",
    "StartDate": "2024-01-15",
    "EndDate": "2024-01-20"
  }
  ```

### 1.3 Configurare Backend

**Locație:** `/backend/Presentation/Program.cs`

Aici găsești configurații importante:

#### **CORS Configuration** (liniile 66-75)
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")  // URL-ul frontend-ului
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Pentru cookie-uri JWT
    });
});
```

#### **Backend URL**
- Development: `http://localhost:5039` (linia 17 din launchSettings.json)
- HTTPS: `https://localhost:7102`

#### **JWT Authentication**
Backend-ul folosește JWT pentru autentificare:
- Token-ul JWT este trimis în răspunsul de la `/api/account/login` și `/api/account/register`
- Token-ul trebuie inclus în cookie-ul `access_token` pentru requesturi autentificate

### 1.4 Database Configuration

**Locație:** `/backend/Presentation/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=RentMyCar;Username=postgres;Password=postgres"
  }
}
```

---

## 2. FRONTEND - Unde să te uiți

### 2.1 Context (State Management)

**Locație:** `/frontend/src/context/AppContext.tsx`

**IMPORTANT:** Momentan, frontend-ul folosește **date DUMMY** (hardcoded). Aici trebuie să faci integrarea!

#### Funcții care trebuie modificate pentru a apela API-ul:

```typescript
// Linia 48 - Login (momentan folosește date locale)
const login = (email: string, password: string): boolean => {
  // AICI trebuie să faci un fetch la: POST http://localhost:5039/api/account/login
  // Cu body: { "UserName": email, "Password": password }
}

// Linia 66 - Register (momentan folosește date locale)
const register = (name: string, email: string, password: string, role: 'client' | 'owner'): boolean => {
  // AICI trebuie să faci un fetch la: POST http://localhost:5039/api/account/register
}

// Linia 99 - Add Car (momentan adaugă local)
const addCar = (carData: Omit<Car, 'id'>) => {
  // AICI trebuie să faci un fetch la: POST http://localhost:5039/api/car
}

// Linia 118 - Create Booking (momentan adaugă local)
const createBooking = (bookingData: Omit<Booking, 'id' | 'createdAt'>): string => {
  // AICI trebuie să faci un fetch la: POST http://localhost:5039/api/booking
}
```

### 2.2 Types (TypeScript Interfaces)

**Locație:** `/frontend/src/types/index.ts`

Aici sunt definite interfețele TypeScript. **ATENȚIE:** Aceste interfețe trebuie să corespundă cu DTOs din backend!

#### Diferențe importante de corectat:

**Frontend User** (linia 1-11):
```typescript
interface User {
  id: string;
  name: string;      // Backend: FirstName + LastName
  email: string;
  password: string;  // Nu ar trebui să fie în frontend!
  role: 'client' | 'owner' | 'both';  // Backend: "User" sau "Owner"
  // Lipsesc: firstName, lastName, city, userName
}
```

**Frontend Car** (linia 13-31):
```typescript
interface Car {
  id: string;
  make: string;        // Backend: Brand
  model: string;
  year: number;
  // Frontend are multe câmpuri care nu există în backend!
  // (image, images, description, features, seats, rating, reviewCount)
}
```

### 2.3 Pages - Exemple de integrare

#### **Login.tsx** (linia 1-106)
```typescript
// Linia 16 - aici se face login
const handleSubmit = (e: React.FormEvent) => {
  e.preventDefault();
  const success = login(email, password);
  // login vine din AppContext și trebuie modificat să apeleze API-ul
};
```

Pentru a integra cu backend-ul:
```typescript
const handleSubmit = async (e: React.FormEvent) => {
  e.preventDefault();
  
  try {
    const response = await fetch('http://localhost:5039/api/account/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include', // Pentru cookie-uri
      body: JSON.stringify({
        UserName: email,  // sau username
        Password: password
      })
    });

    if (response.ok) {
      const data = await response.json();
      // data conține: { UserName, Email, FirstName, LastName, City, Token }
      // Salvează token-ul și datele utilizatorului
      document.cookie = `access_token=${data.Token}; path=/`;
      // Actualizează state-ul aplicației
    }
  } catch (error) {
    console.error('Login failed:', error);
  }
};
```

---

## 3. PAȘI DE INTEGRARE

### Pas 1: Configurare Frontend pentru API Calls

Creează un fișier `/frontend/src/config/api.ts`:

```typescript
const API_BASE_URL = 'http://localhost:5039';

export const apiConfig = {
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  credentials: 'include' as RequestCredentials, // Pentru cookie-uri JWT
};

export const endpoints = {
  // Account
  register: `${API_BASE_URL}/api/account/register`,
  login: `${API_BASE_URL}/api/account/login`,
  updateUser: `${API_BASE_URL}/api/account/update`,
  
  // Cars
  cars: `${API_BASE_URL}/api/car`,
  carById: (id: string) => `${API_BASE_URL}/api/car/${id}`,
  
  // Bookings
  bookings: `${API_BASE_URL}/api/booking`,
  bookingById: (id: string) => `${API_BASE_URL}/api/booking/${id}`,
  acceptOrReject: (id: string) => `${API_BASE_URL}/api/booking/${id}/accept-or-reject`,
  userHistory: `${API_BASE_URL}/get-user-history`,
  ownerHistory: `${API_BASE_URL}/get-owner-history`,
  
  // Reviews
  reviewByBooking: (bookingId: string) => `${API_BASE_URL}/api/review/${bookingId}`,
};
```

### Pas 2: Creează servicii API

Creează `/frontend/src/services/apiService.ts`:

```typescript
import { apiConfig, endpoints } from '../config/api';

export const apiService = {
  // Login
  async login(username: string, password: string) {
    const response = await fetch(endpoints.login, {
      method: 'POST',
      headers: apiConfig.headers,
      credentials: apiConfig.credentials,
      body: JSON.stringify({ UserName: username, Password: password }),
    });
    
    if (!response.ok) throw new Error('Login failed');
    return response.json();
  },

  // Register
  async register(data: {
    email: string;
    password: string;
    userName: string;
    firstName: string;
    lastName: string;
    city: string;
    role: string;
  }) {
    const response = await fetch(endpoints.register, {
      method: 'POST',
      headers: apiConfig.headers,
      credentials: apiConfig.credentials,
      body: JSON.stringify(data),
    });
    
    if (!response.ok) throw new Error('Registration failed');
    return response.json();
  },

  // Get all cars
  async getCars(filters?: {
    city?: string;
    minPrice?: number;
    maxPrice?: number;
    brand?: string;
    model?: string;
  }) {
    const params = new URLSearchParams();
    if (filters) {
      Object.entries(filters).forEach(([key, value]) => {
        if (value) params.append(key, value.toString());
      });
    }
    
    const url = filters && params.toString() 
      ? `${endpoints.cars}?${params.toString()}`
      : endpoints.cars;
    
    const response = await fetch(url, {
      credentials: apiConfig.credentials,
    });
    
    if (!response.ok) throw new Error('Failed to fetch cars');
    return response.json();
  },

  // Add more methods for other endpoints...
};
```

### Pas 3: Modifică AppContext pentru a folosi API-ul

În `/frontend/src/context/AppContext.tsx`, înlocuiește funcțiile cu apeluri către API:

```typescript
const login = async (email: string, password: string): Promise<boolean> => {
  try {
    const data = await apiService.login(email, password);
    
    // Salvează token în cookie (backend-ul se așteaptă la asta)
    document.cookie = `access_token=${data.Token}; path=/`;
    
    // Transformă datele din backend în format frontend
    const user: User = {
      id: data.Email, // sau alt identificator
      name: `${data.FirstName} ${data.LastName}`,
      email: data.Email,
      role: data.Role.toLowerCase(),
      // ... alte câmpuri
    };
    
    setCurrentUser(user);
    localStorage.setItem('currentUser', JSON.stringify(user));
    addToast('success', `Welcome back, ${user.name}!`);
    return true;
  } catch (error) {
    addToast('error', 'Invalid email or password');
    return false;
  }
};
```

### Pas 4: Adaugă Vite Proxy (Opțional, pentru a evita CORS)

În `/frontend/vite.config.ts`:

```typescript
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5039',
        changeOrigin: true,
      },
    },
  },
  optimizeDeps: {
    exclude: ['lucide-react'],
  },
});
```

Apoi folosește `/api/...` în loc de `http://localhost:5039/api/...`

---

## 4. CHECKLIST INTEGRARE

- [ ] **Backend-ul rulează** pe `http://localhost:5039`
- [ ] **Baza de date** este configurată și migrată
- [ ] **CORS** este configurat corect în `Program.cs`
- [ ] Creează fișierul **`/frontend/src/config/api.ts`** cu endpoints
- [ ] Creează fișierul **`/frontend/src/services/apiService.ts`** cu funcții API
- [ ] Modifică **`AppContext.tsx`** pentru a folosi `apiService`
- [ ] Actualizează **types** din `/frontend/src/types/index.ts` să corespundă cu DTOs
- [ ] Testează **login** și **register**
- [ ] Testează **listarea mașinilor**
- [ ] Testează **creare rezervare**
- [ ] Adaugă **error handling** pentru toate requesturile
- [ ] Adaugă **loading states** în componente

---

## 5. PROBLEME COMUNE ȘI SOLUȚII

### Problemă: CORS Error
**Soluție:** Verifică că backend-ul rulează și că CORS este configurat corect în `Program.cs` (linia 93).

### Problemă: 401 Unauthorized
**Soluție:** Verifică că token-ul JWT este salvat în cookie-ul `access_token` și că este trimis cu fiecare request (`credentials: 'include'`).

### Problemă: Date incompatibile între frontend și backend
**Soluție:** Creează funcții mapper pentru a transforma DTOs backend în interfețe frontend:

```typescript
// Exemple de mappere
const mapUserDTOToUser = (dto: NewUserDTO): User => ({
  id: dto.Email,
  name: `${dto.FirstName} ${dto.LastName}`,
  email: dto.Email,
  role: dto.Role.toLowerCase() as 'client' | 'owner',
  // ... alte câmpuri
});

const mapCarDTOToCar = (dto: CarDTO): Car => ({
  id: dto.Id,
  make: dto.Brand,
  model: dto.Model,
  year: dto.Year,
  pricePerDay: dto.PricePerDay,
  city: dto.City,
  // ... alte câmpuri
});
```

---

## 6. COMENZI UTILE

### Backend
```bash
cd backend
dotnet restore
dotnet ef database update --project Domain --startup-project Presentation
dotnet run --project Presentation
```

### Frontend
```bash
cd frontend
npm install
npm run dev
```

---

## Concluzie

Pentru a lega frontend-ul de backend, principalele fișiere pe care trebuie să te concentrezi sunt:

1. **Backend Controllers** - pentru a vedea ce endpoints sunt disponibile
2. **Backend DTOs** - pentru a ști ce date se așteaptă/returnează API-ul
3. **Frontend AppContext.tsx** - unde trebuie să înlocuiești date dummy cu apeluri API
4. **Frontend types/index.ts** - pentru a alinia interfețele cu DTOs

Succes la integrare! 🚀

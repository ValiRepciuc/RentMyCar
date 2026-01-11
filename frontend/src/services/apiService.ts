import { apiConfig, endpoints } from '../config/api';
import { Car } from '../types';

export const apiService = {
  // ============ Account / Auth ============
  async login(username: string, password: string) {
    const response = await fetch(endpoints.login, {
      method: 'POST',
      headers: apiConfig.headers,
      credentials: apiConfig.credentials,
      body: JSON.stringify({ UserName: username, Password: password }),
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Login failed');
    }
    return response.json();
  },

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
      body: JSON.stringify({
        Email: data.email,
        Password: data.password,
        UserName: data.userName,
        FirstName: data.firstName,
        LastName: data.lastName,
        City: data.city,
        Role: data.role,
      }),
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Registration failed');
    }
    return response.json();
  },

  // ============ Cars ============
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
        if (value !== undefined && value !== null && value !== '') {
          params.append(key, value.toString());
        }
      });
    }
    
    const url = params.toString() 
      ? `${endpoints.cars}?${params.toString()}`
      : endpoints.cars;
    
    const response = await fetch(url, {
      credentials: apiConfig.credentials,
    });
    
    if (!response.ok) {
      throw new Error('Failed to fetch cars');
    }
    return response.json();
  },

  async getCarById(id: string) {
    const response = await fetch(endpoints.carById(id), {
      credentials: apiConfig.credentials,
    });
    
    if (!response.ok) {
      throw new Error('Failed to fetch car details');
    }
    return response.json();
  },

  async createCar(carData: {
    brand: string;
    model: string;
    year: number;
    pricePerDay: number;
    city: string;
    fuelType: string;
    transmission: string;
    description?: string;
    features?: string[];
    imageUrl?: string;
    imageUrls?: string[];
    seats?: number;
  }) {
    const response = await fetch(endpoints.cars, {
      method: 'POST',
      headers: apiConfig.headers,
      credentials: apiConfig.credentials,
      body: JSON.stringify({
        Brand: carData.brand,
        Model: carData.model,
        Year: carData.year,
        PricePerDay: carData.pricePerDay,
        City: carData.city,
        FuelType: carData.fuelType,
        Transmission: carData.transmission,
        Description: carData.description || '',
        Features: carData.features || [],
        ImageUrl: carData.imageUrl || '',
        ImageUrls: carData.imageUrls || [],
        Seats: carData.seats || 5,
      }),
    });
    
    if (!response.ok) {
      throw new Error('Failed to create car');
    }
    return response.json();
  },

  // ============ Bookings ============
  async createBooking(bookingData: {
    carId: string;
    startDate: string;
    endDate: string;
  }) {
    const response = await fetch(endpoints.bookings, {
      method: 'POST',
      headers: apiConfig.headers,
      credentials: apiConfig.credentials,
      body: JSON.stringify({
        CarId: bookingData.carId,
        StartDate: bookingData.startDate,
        EndDate: bookingData.endDate,
      }),
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to create booking');
    }
    return response.json();
  },

  async getBookings() {
    const response = await fetch(endpoints.bookings, {
      credentials: apiConfig.credentials,
    });
    
    if (!response.ok) {
      throw new Error('Failed to fetch bookings');
    }
    return response.json();
  },

  async getBookingById(id: string) {
    const response = await fetch(endpoints.bookingById(id), {
      credentials: apiConfig.credentials,
    });
    
    if (!response.ok) {
      throw new Error('Failed to fetch booking');
    }
    return response.json();
  },

  async getUserHistory() {
    const response = await fetch(endpoints.userHistory, {
      credentials: apiConfig.credentials,
    });
    
    if (!response.ok) {
      throw new Error('Failed to fetch user history');
    }
    return response.json();
  },

  async getOwnerHistory() {
    const response = await fetch(endpoints.ownerHistory, {
      credentials: apiConfig.credentials,
    });
    
    if (!response.ok) {
      throw new Error('Failed to fetch owner history');
    }
    return response.json();
  },
};

// Helper function to map backend Car DTO to frontend Car type
export function mapCarDTOToCar(dto: any): Car {
  return {
    id: dto.Id,
    ownerId: dto.OwnerId,
    make: dto.Brand,
    model: dto.Model,
    year: dto.Year,
    city: dto.City,
    pricePerDay: dto.PricePerDay,
    image: dto.ImageUrl || '',
    images: dto.ImageUrls || [],
    description: dto.Description || '',
    features: dto.Features || [],
    transmission: dto.Transmission.toLowerCase() as 'automatic' | 'manual',
    fuelType: dto.FuelType.toLowerCase() as 'gasoline' | 'diesel' | 'electric' | 'hybrid',
    seats: dto.Seats || 5,
    available: dto.IsActive,
    rating: dto.Rating || 0,
    reviewCount: dto.ReviewCount || 0,
  };
}

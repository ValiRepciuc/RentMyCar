import { apiConfig, endpoints } from '../config/api';

// Backend DTOs interfaces
interface NewUserDTO {
  Email: string;
  FirstName: string;
  LastName: string;
  UserName: string;
  City: string;
  Role: string;
  Token: string;
}

interface CarDTO {
  Id: string;
  Brand: string;
  Model: string;
  Year: number;
  PricePerDay: number;
  City: string;
  FuelType: string;
  Transmission: string;
  IsActive: boolean;
  OwnerId: string;
  OwnerName: string;
}

interface BookingDTO {
  Id: string;
  CarId: string;
  CarBrand: string;
  CarModel: string;
  RenterId: string;
  RenterName: string;
  StartDate: string;
  EndDate: string;
  TotalPrice: number;
  Status: string;
}

interface ReviewDTO {
  Id: string;
  BookingId: string;
  Rating: number;
  Comment: string;
  ReviewerName: string;
  CreatedAt: string;
}

export const apiService = {
  // Login
  async login(username: string, password: string): Promise<NewUserDTO> {
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

  // Register
  async register(data: {
    email: string;
    password: string;
    userName: string;
    firstName: string;
    lastName: string;
    city: string;
    role: string;
  }): Promise<NewUserDTO> {
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

  // Update user profile
  async updateUser(data: {
    userName: string;
    firstName: string;
    lastName: string;
    city: string;
  }): Promise<void> {
    const response = await fetch(endpoints.updateUser, {
      method: 'PUT',
      headers: apiConfig.headers,
      credentials: apiConfig.credentials,
      body: JSON.stringify({
        UserName: data.userName,
        FirstName: data.firstName,
        LastName: data.lastName,
        City: data.city,
      }),
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Update failed');
    }
  },

  // Get all cars
  async getCars(filters?: {
    city?: string;
    minPrice?: number;
    maxPrice?: number;
    brand?: string;
    model?: string;
  }): Promise<CarDTO[]> {
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
      const error = await response.text();
      throw new Error(error || 'Failed to fetch cars');
    }
    return response.json();
  },

  // Get car by ID
  async getCarById(id: string): Promise<CarDTO> {
    const response = await fetch(endpoints.carById(id), {
      credentials: apiConfig.credentials,
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to fetch car');
    }
    return response.json();
  },

  // Add car
  async addCar(data: {
    brand: string;
    model: string;
    year: number;
    pricePerDay: number;
    city: string;
    fuelType: string;
    transmission: string;
  }): Promise<CarDTO> {
    const response = await fetch(endpoints.cars, {
      method: 'POST',
      headers: apiConfig.headers,
      credentials: apiConfig.credentials,
      body: JSON.stringify({
        Brand: data.brand,
        Model: data.model,
        Year: data.year,
        PricePerDay: data.pricePerDay,
        City: data.city,
        FuelType: data.fuelType,
        Transmission: data.transmission,
      }),
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to add car');
    }
    return response.json();
  },

  // Update car
  async updateCar(id: string, data: {
    brand?: string;
    model?: string;
    year?: number;
    pricePerDay?: number;
    city?: string;
    fuelType?: string;
    transmission?: string;
    isActive?: boolean;
  }): Promise<CarDTO> {
    const response = await fetch(endpoints.carById(id), {
      method: 'PUT',
      headers: apiConfig.headers,
      credentials: apiConfig.credentials,
      body: JSON.stringify({
        Brand: data.brand,
        Model: data.model,
        Year: data.year,
        PricePerDay: data.pricePerDay,
        City: data.city,
        FuelType: data.fuelType,
        Transmission: data.transmission,
        IsActive: data.isActive,
      }),
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to update car');
    }
    return response.json();
  },

  // Delete car
  async deleteCar(id: string): Promise<void> {
    const response = await fetch(endpoints.carById(id), {
      method: 'DELETE',
      credentials: apiConfig.credentials,
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to delete car');
    }
  },

  // Get all bookings
  async getBookings(): Promise<BookingDTO[]> {
    const response = await fetch(endpoints.bookings, {
      credentials: apiConfig.credentials,
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to fetch bookings');
    }
    return response.json();
  },

  // Get booking by ID
  async getBookingById(id: string): Promise<BookingDTO> {
    const response = await fetch(endpoints.bookingById(id), {
      credentials: apiConfig.credentials,
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to fetch booking');
    }
    return response.json();
  },

  // Create booking
  async createBooking(data: {
    carId: string;
    startDate: string;
    endDate: string;
  }): Promise<BookingDTO> {
    const response = await fetch(endpoints.bookings, {
      method: 'POST',
      headers: apiConfig.headers,
      credentials: apiConfig.credentials,
      body: JSON.stringify({
        CarId: data.carId,
        StartDate: data.startDate,
        EndDate: data.endDate,
      }),
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to create booking');
    }
    return response.json();
  },

  // Update booking
  async updateBooking(id: string, data: {
    startDate?: string;
    endDate?: string;
  }): Promise<BookingDTO> {
    const response = await fetch(endpoints.bookingById(id), {
      method: 'PUT',
      headers: apiConfig.headers,
      credentials: apiConfig.credentials,
      body: JSON.stringify({
        StartDate: data.startDate,
        EndDate: data.endDate,
      }),
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to update booking');
    }
    return response.json();
  },

  // Accept or reject booking
  async acceptOrRejectBooking(id: string, accept: boolean): Promise<void> {
    const response = await fetch(endpoints.acceptOrReject(id), {
      method: 'PUT',
      headers: apiConfig.headers,
      credentials: apiConfig.credentials,
      body: JSON.stringify({ Accept: accept }),
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to update booking status');
    }
  },

  // Delete booking (cancel)
  async deleteBooking(id: string): Promise<void> {
    const response = await fetch(endpoints.bookingById(id), {
      method: 'DELETE',
      credentials: apiConfig.credentials,
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to delete booking');
    }
  },

  // Get user booking history
  async getUserHistory(): Promise<BookingDTO[]> {
    const response = await fetch(endpoints.userHistory, {
      credentials: apiConfig.credentials,
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to fetch user history');
    }
    return response.json();
  },

  // Get owner booking history
  async getOwnerHistory(): Promise<BookingDTO[]> {
    const response = await fetch(endpoints.ownerHistory, {
      credentials: apiConfig.credentials,
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to fetch owner history');
    }
    return response.json();
  },

  // Get review by booking ID
  async getReviewByBooking(bookingId: string): Promise<ReviewDTO> {
    const response = await fetch(endpoints.reviewByBooking(bookingId), {
      credentials: apiConfig.credentials,
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to fetch review');
    }
    return response.json();
  },

  // Create review
  async createReview(bookingId: string, data: {
    rating: number;
    comment: string;
  }): Promise<ReviewDTO> {
    const response = await fetch(endpoints.reviewByBooking(bookingId), {
      method: 'POST',
      headers: apiConfig.headers,
      credentials: apiConfig.credentials,
      body: JSON.stringify({
        Rating: data.rating,
        Comment: data.comment,
      }),
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to create review');
    }
    return response.json();
  },
};

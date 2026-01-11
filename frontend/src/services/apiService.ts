import { apiConfig, endpoints } from '../config/api';

// Backend DTOs interfaces
interface NewUserDTO {
  email: string;
  firstName: string;
  lastName: string;
  userName: string;
  city: string;
  role: string;
  token: string;
}

interface CarDTO {
  id: string;
  brand: string;
  model: string;
  year: number;
  pricePerDay: number;
  city: string;
  fuelType: string;
  transmission: string;
  isActive: boolean;
  ownerId: string;
  ownerName: string;
}

interface BookingDTO {
  id: string;
  carId: string;
  carBrand: string;
  carModel: string;
  renterId: string;
  renterName: string;
  startDate: string;
  endDate: string;
  totalPrice: number;
  status: string;
}

interface ReviewDTO {
  id: string;
  bookingId: string;
  rating: number;
  comment: string;
  reviewerName: string;
  createdAt: string;
}

export const apiService = {
  // Login
  async login(username: string, password: string): Promise<NewUserDTO> {
    const response = await fetch(endpoints.login, {
      method: 'POST',
      headers: apiConfig.headers,
      credentials: apiConfig.credentials,
      body: JSON.stringify({ userName: username, password: password }),
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
        email: data.email,
        password: data.password,
        userName: data.userName,
        firstName: data.firstName,
        lastName: data.lastName,
        city: data.city,
        role: data.role,
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
        userName: data.userName,
        firstName: data.firstName,
        lastName: data.lastName,
        city: data.city,
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
        brand: data.brand,
        model: data.model,
        year: data.year,
        pricePerDay: data.pricePerDay,
        city: data.city,
        fuelType: data.fuelType,
        transmission: data.transmission,
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
        brand: data.brand,
        model: data.model,
        year: data.year,
        pricePerDay: data.pricePerDay,
        city: data.city,
        fuelType: data.fuelType,
        transmission: data.transmission,
        isActive: data.isActive,
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
        carId: data.carId,
        startDate: data.startDate,
        endDate: data.endDate,
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
        startDate: data.startDate,
        endDate: data.endDate,
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
      body: JSON.stringify({ accept: accept }),
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
        rating: data.rating,
        comment: data.comment,
      }),
    });
    
    if (!response.ok) {
      const error = await response.text();
      throw new Error(error || 'Failed to create review');
    }
    return response.json();
  },
};

export interface User {
  id: string;
  name: string;
  email: string;
  password: string;
  role: 'client' | 'owner' | 'both';
  avatar: string;
  phone?: string;
  bio?: string;
  createdAt: string;
}

export interface Car {
  id: string;
  ownerId: string;
  make: string;
  model: string;
  year: number;
  city: string;
  pricePerDay: number;
  image: string;
  images: string[];
  description: string;
  features: string[];
  transmission: 'automatic' | 'manual';
  fuelType: 'gasoline' | 'diesel' | 'electric' | 'hybrid';
  seats: number;
  available: boolean;
  rating: number;
  reviewCount: number;
}

export interface Booking {
  id: string;
  carId: string;
  clientId: string;
  ownerId: string;
  startDate: string;
  endDate: string;
  totalPrice: number;
  status: 'pending' | 'accepted' | 'rejected' | 'completed' | 'cancelled';
  createdAt: string;
}

export interface Review {
  id: string;
  bookingId: string;
  carId: string;
  clientId: string;
  rating: number;
  comment: string;
  createdAt: string;
}

export type ToastType = 'success' | 'error' | 'info' | 'warning';

export interface Toast {
  id: string;
  type: ToastType;
  message: string;
}

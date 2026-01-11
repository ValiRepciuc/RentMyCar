import { createContext, useContext, useState, ReactNode, useEffect } from 'react';
import { User, Car, Booking, Review, Toast } from '../types';
import { users as initialUsers, cars as initialCars, bookings as initialBookings, reviews as initialReviews } from '../data/dummyData';
import { apiService } from '../services/apiService';

interface AppContextType {
  currentUser: User | null;
  users: User[];
  cars: Car[];
  bookings: Booking[];
  reviews: Review[];
  toasts: Toast[];
  login: (email: string, password: string) => Promise<boolean>;
  logout: () => void;
  register: (name: string, email: string, password: string, role: 'client' | 'owner') => Promise<boolean>;
  updateUser: (updates: Partial<User>) => Promise<void>;
  addCar: (car: Omit<Car, 'id'>) => Promise<void>;
  updateCar: (carId: string, updates: Partial<Car>) => Promise<void>;
  deleteCar: (carId: string) => Promise<void>;
  createBooking: (booking: Omit<Booking, 'id' | 'createdAt'>) => Promise<string>;
  updateBooking: (bookingId: string, updates: Partial<Booking>) => Promise<void>;
  cancelBooking: (bookingId: string) => Promise<void>;
  addReview: (review: Omit<Review, 'id' | 'createdAt'>) => Promise<void>;
  addToast: (type: Toast['type'], message: string) => void;
  removeToast: (id: string) => void;
  loadCars: () => Promise<void>;
  loadBookings: () => Promise<void>;
}

const AppContext = createContext<AppContextType | undefined>(undefined);

export const AppProvider = ({ children }: { children: ReactNode }) => {
  const [currentUser, setCurrentUser] = useState<User | null>(null);
  const [users, setUsers] = useState<User[]>(initialUsers);
  const [cars, setCars] = useState<Car[]>(initialCars);
  const [bookings, setBookings] = useState<Booking[]>(initialBookings);
  const [reviews, setReviews] = useState<Review[]>(initialReviews);
  const [toasts, setToasts] = useState<Toast[]>([]);

  const loadCars = async () => {
    try {
      const carsData = await apiService.getCars();
      const mappedCars: Car[] = carsData.map(carDto => ({
        id: carDto.Id,
        ownerId: carDto.OwnerId,
        make: carDto.Brand,
        model: carDto.Model,
        year: carDto.Year,
        city: carDto.City,
        pricePerDay: carDto.PricePerDay,
        image: 'https://images.unsplash.com/photo-1494976388531-d1058494cdd8?auto=format&fit=crop&q=80',
        images: ['https://images.unsplash.com/photo-1494976388531-d1058494cdd8?auto=format&fit=crop&q=80'],
        description: `${carDto.Brand} ${carDto.Model} ${carDto.Year}`,
        features: [carDto.Transmission, carDto.FuelType],
        transmission: carDto.Transmission.toLowerCase() === 'automatic' ? 'automatic' : 'manual',
        fuelType: carDto.FuelType.toLowerCase() as 'gasoline' | 'diesel' | 'electric' | 'hybrid',
        seats: 5,
        available: carDto.IsActive,
        rating: 4.5,
        reviewCount: 0,
      }));
      setCars(mappedCars);
    } catch (error) {
      console.error('Failed to load cars:', error);
      // Keep using dummy data if API fails
    }
  };

  const loadBookings = async () => {
    try {
      if (!currentUser) return;
      const bookingsData = await apiService.getBookings();
      const mappedBookings: Booking[] = bookingsData.map(bookingDto => ({
        id: bookingDto.Id,
        carId: bookingDto.CarId,
        clientId: bookingDto.RenterId,
        ownerId: '', // Not provided by backend
        startDate: bookingDto.StartDate,
        endDate: bookingDto.EndDate,
        totalPrice: bookingDto.TotalPrice,
        status: bookingDto.Status.toLowerCase() as 'pending' | 'accepted' | 'rejected' | 'completed' | 'cancelled',
        createdAt: new Date().toISOString(),
      }));
      setBookings(mappedBookings);
    } catch (error) {
      console.error('Failed to load bookings:', error);
      // Keep using dummy data if API fails
    }
  };

  useEffect(() => {
    const savedUser = localStorage.getItem('currentUser');
    if (savedUser) {
      const user = JSON.parse(savedUser);
      setCurrentUser(user);
    }
    // Load initial data from API
    loadCars();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const login = async (email: string, password: string): Promise<boolean> => {
    try {
      const data = await apiService.login(email, password);
      
      // Save token in cookie (backend expects this)
      document.cookie = `access_token=${data.Token}; path=/`;
      
      // Transform backend data to frontend User format
      const user: User = {
        id: data.Email,
        name: `${data.FirstName} ${data.LastName}`,
        email: data.Email,
        password: '', // Don't store password
        role: data.Role.toLowerCase() === 'owner' ? 'owner' : 'client',
        avatar: 'https://images.pexels.com/photos/1036623/pexels-photo-1036623.jpeg?auto=compress&cs=tinysrgb&w=200',
        createdAt: new Date().toISOString(),
      };
      
      setCurrentUser(user);
      localStorage.setItem('currentUser', JSON.stringify(user));
      addToast('success', `Welcome back, ${user.name}!`);
      
      // Load user-specific data
      await loadCars();
      await loadBookings();
      
      return true;
    } catch (error) {
      addToast('error', 'Invalid email or password');
      return false;
    }
  };

  const logout = () => {
    setCurrentUser(null);
    localStorage.removeItem('currentUser');
    document.cookie = 'access_token=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT';
    addToast('info', 'You have been logged out');
  };

  const register = async (name: string, email: string, password: string, role: 'client' | 'owner'): Promise<boolean> => {
    try {
      // Split name into first and last name
      const nameParts = name.trim().split(' ');
      const firstName = nameParts[0] || name;
      const lastName = nameParts.slice(1).join(' ') || nameParts[0];
      
      const data = await apiService.register({
        email,
        password,
        userName: email.split('@')[0],
        firstName,
        lastName,
        city: 'Unknown', // Default city, can be updated later
        role: role === 'owner' ? 'Owner' : 'User',
      });
      
      // Save token in cookie
      document.cookie = `access_token=${data.Token}; path=/`;
      
      // Transform backend data to frontend User format
      const newUser: User = {
        id: data.Email,
        name: `${data.FirstName} ${data.LastName}`,
        email: data.Email,
        password: '',
        role: data.Role.toLowerCase() === 'owner' ? 'owner' : 'client',
        avatar: 'https://images.pexels.com/photos/1036623/pexels-photo-1036623.jpeg?auto=compress&cs=tinysrgb&w=200',
        createdAt: new Date().toISOString(),
      };
      
      setCurrentUser(newUser);
      localStorage.setItem('currentUser', JSON.stringify(newUser));
      addToast('success', 'Account created successfully!');
      
      return true;
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : 'Registration failed';
      addToast('error', errorMessage);
      return false;
    }
  };

  const updateUser = async (updates: Partial<User>): Promise<void> => {
    if (!currentUser) return;

    try {
      // Map frontend User fields to backend fields
      const nameParts = (updates.name || currentUser.name).split(' ');
      const firstName = nameParts[0] || currentUser.name;
      const lastName = nameParts.slice(1).join(' ') || nameParts[0];
      
      await apiService.updateUser({
        userName: currentUser.email.split('@')[0],
        firstName,
        lastName,
        city: updates.phone || 'Unknown', // Using phone as placeholder since backend expects city
      });

      const updatedUser = { ...currentUser, ...updates };
      setCurrentUser(updatedUser);
      localStorage.setItem('currentUser', JSON.stringify(updatedUser));
      addToast('success', 'Profile updated successfully');
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : 'Update failed';
      addToast('error', errorMessage);
    }
  };

  const addCar = async (carData: Omit<Car, 'id'>): Promise<void> => {
    try {
      const carDto = await apiService.addCar({
        brand: carData.make,
        model: carData.model,
        year: carData.year,
        pricePerDay: carData.pricePerDay,
        city: carData.city,
        fuelType: carData.fuelType.charAt(0).toUpperCase() + carData.fuelType.slice(1),
        transmission: carData.transmission.charAt(0).toUpperCase() + carData.transmission.slice(1),
      });

      const newCar: Car = {
        id: carDto.Id,
        ownerId: carDto.OwnerId,
        make: carDto.Brand,
        model: carDto.Model,
        year: carDto.Year,
        city: carDto.City,
        pricePerDay: carDto.PricePerDay,
        image: carData.image,
        images: carData.images,
        description: carData.description,
        features: carData.features,
        transmission: carDto.Transmission.toLowerCase() === 'automatic' ? 'automatic' : 'manual',
        fuelType: carDto.FuelType.toLowerCase() as 'gasoline' | 'diesel' | 'electric' | 'hybrid',
        seats: carData.seats,
        available: carDto.IsActive,
        rating: carData.rating,
        reviewCount: carData.reviewCount,
      };
      
      setCars([...cars, newCar]);
      addToast('success', 'Car added successfully');
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : 'Failed to add car';
      addToast('error', errorMessage);
    }
  };

  const updateCar = async (carId: string, updates: Partial<Car>): Promise<void> => {
    try {
      await apiService.updateCar(carId, {
        brand: updates.make,
        model: updates.model,
        year: updates.year,
        pricePerDay: updates.pricePerDay,
        city: updates.city,
        fuelType: updates.fuelType ? updates.fuelType.charAt(0).toUpperCase() + updates.fuelType.slice(1) : undefined,
        transmission: updates.transmission ? updates.transmission.charAt(0).toUpperCase() + updates.transmission.slice(1) : undefined,
        isActive: updates.available,
      });

      setCars(cars.map(car => car.id === carId ? { ...car, ...updates } : car));
      addToast('success', 'Car updated successfully');
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : 'Failed to update car';
      addToast('error', errorMessage);
    }
  };

  const deleteCar = async (carId: string): Promise<void> => {
    try {
      await apiService.deleteCar(carId);
      setCars(cars.filter(car => car.id !== carId));
      addToast('success', 'Car deleted successfully');
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : 'Failed to delete car';
      addToast('error', errorMessage);
    }
  };

  const createBooking = async (bookingData: Omit<Booking, 'id' | 'createdAt'>): Promise<string> => {
    try {
      const bookingDto = await apiService.createBooking({
        carId: bookingData.carId,
        startDate: bookingData.startDate,
        endDate: bookingData.endDate,
      });

      const newBooking: Booking = {
        id: bookingDto.Id,
        carId: bookingDto.CarId,
        clientId: bookingDto.RenterId,
        ownerId: bookingData.ownerId,
        startDate: bookingDto.StartDate,
        endDate: bookingDto.EndDate,
        totalPrice: bookingDto.TotalPrice,
        status: bookingDto.Status.toLowerCase() as 'pending' | 'accepted' | 'rejected' | 'completed' | 'cancelled',
        createdAt: new Date().toISOString(),
      };
      
      setBookings([...bookings, newBooking]);
      addToast('success', 'Booking request sent!');
      return newBooking.id;
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : 'Failed to create booking';
      addToast('error', errorMessage);
      throw error;
    }
  };

  const updateBooking = async (bookingId: string, updates: Partial<Booking>): Promise<void> => {
    try {
      // Handle status updates differently (accept/reject)
      if (updates.status === 'accepted') {
        await apiService.acceptOrRejectBooking(bookingId, true);
        setBookings(bookings.map(b => b.id === bookingId ? { ...b, status: 'accepted' } : b));
        addToast('success', 'Booking accepted!');
      } else if (updates.status === 'rejected') {
        await apiService.acceptOrRejectBooking(bookingId, false);
        setBookings(bookings.map(b => b.id === bookingId ? { ...b, status: 'rejected' } : b));
        addToast('error', 'Booking rejected');
      } else if (updates.startDate || updates.endDate) {
        // Update booking dates
        await apiService.updateBooking(bookingId, {
          startDate: updates.startDate,
          endDate: updates.endDate,
        });
        setBookings(bookings.map(b => b.id === bookingId ? { ...b, ...updates } : b));
        addToast('success', 'Booking updated!');
      }
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : 'Failed to update booking';
      addToast('error', errorMessage);
    }
  };

  const cancelBooking = async (bookingId: string): Promise<void> => {
    try {
      await apiService.deleteBooking(bookingId);
      setBookings(bookings.map(b => b.id === bookingId ? { ...b, status: 'cancelled' as const } : b));
      addToast('info', 'Booking cancelled');
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : 'Failed to cancel booking';
      addToast('error', errorMessage);
    }
  };

  const addReview = async (reviewData: Omit<Review, 'id' | 'createdAt'>): Promise<void> => {
    try {
      const reviewDto = await apiService.createReview(reviewData.bookingId, {
        rating: reviewData.rating,
        comment: reviewData.comment,
      });

      const newReview: Review = {
        id: reviewDto.Id,
        bookingId: reviewDto.BookingId,
        carId: reviewData.carId,
        clientId: reviewData.clientId,
        rating: reviewDto.Rating,
        comment: reviewDto.Comment,
        createdAt: reviewDto.CreatedAt,
      };
      
      setReviews([...reviews, newReview]);

      const car = cars.find(c => c.id === reviewData.carId);
      if (car) {
        const carReviews = reviews.filter(r => r.carId === car.id);
        const totalRating = carReviews.reduce((sum, r) => sum + r.rating, 0) + reviewData.rating;
        const newRating = totalRating / (carReviews.length + 1);

        await updateCar(car.id, {
          rating: Math.round(newRating * 10) / 10,
          reviewCount: car.reviewCount + 1,
        });
      }

      addToast('success', 'Review submitted successfully');
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : 'Failed to submit review';
      addToast('error', errorMessage);
    }
  };

  const addToast = (type: Toast['type'], message: string) => {
    const id = `toast-${Date.now()}`;
    const newToast: Toast = { id, type, message };
    setToasts(prev => [...prev, newToast]);

    setTimeout(() => {
      removeToast(id);
    }, 5000);
  };

  const removeToast = (id: string) => {
    setToasts(prev => prev.filter(t => t.id !== id));
  };

  return (
    <AppContext.Provider
      value={{
        currentUser,
        users,
        cars,
        bookings,
        reviews,
        toasts,
        login,
        logout,
        register,
        updateUser,
        addCar,
        updateCar,
        deleteCar,
        createBooking,
        updateBooking,
        cancelBooking,
        addReview,
        addToast,
        removeToast,
        loadCars,
        loadBookings,
      }}
    >
      {children}
    </AppContext.Provider>
  );
};

export const useApp = () => {
  const context = useContext(AppContext);
  if (context === undefined) {
    throw new Error('useApp must be used within an AppProvider');
  }
  return context;
};

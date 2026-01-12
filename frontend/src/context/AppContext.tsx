import { createContext, useContext, useState, ReactNode, useEffect } from 'react';
import { User, Car, Booking, Review, Toast } from '../types';
import { apiService, mapCarDTOToCar } from '../services/apiService';

interface AppContextType {
  currentUser: User | null;
  users: User[];
  cars: Car[];
  bookings: Booking[];
  reviews: Review[];
  toasts: Toast[];
  loading: boolean;
  login: (email: string, password: string) => Promise<boolean>;
  logout: () => void;
  register: (name: string, email: string, password: string, role: 'client' | 'owner') => Promise<boolean>;
  updateUser: (updates: Partial<User>) => Promise<void>;
  addCar: (car: Omit<Car, 'id'>) => Promise<void>;
  updateCar: (carId: string, updates: Partial<Car>) => void;
  deleteCar: (carId: string) => void;
  createBooking: (booking: Omit<Booking, 'id' | 'createdAt'>) => Promise<string>;
  updateBooking: (bookingId: string, updates: Partial<Booking>) => void;
  cancelBooking: (bookingId: string) => void;
  addReview: (review: Omit<Review, 'id' | 'createdAt'>) => void;
  addToast: (type: Toast['type'], message: string) => void;
  removeToast: (id: string) => void;
  fetchCars: (filters?: { city?: string; minPrice?: number; maxPrice?: number; brand?: string; model?: string }) => Promise<void>;
}

const AppContext = createContext<AppContextType | undefined>(undefined);

export const AppProvider = ({ children }: { children: ReactNode }) => {
  const [currentUser, setCurrentUser] = useState<User | null>(null);
  const [users, setUsers] = useState<User[]>([]);
  const [cars, setCars] = useState<Car[]>([]);
  const [bookings, setBookings] = useState<Booking[]>([]);
  const [reviews, setReviews] = useState<Review[]>([]);
  const [toasts, setToasts] = useState<Toast[]>([]);
  const [loading, setLoading] = useState<boolean>(false);

  // Load cars on mount
  useEffect(() => {
    fetchCars();
    
    // Try to restore user from cookie by fetching current user from backend
    const checkAuth = async () => {
      try {
        const userData = await apiService.getCurrentUser();
        const backendRole = userData.Role || 'User';
        const frontendRole = backendRole.toLowerCase() === 'owner' ? 'owner' : 'client';
        
        const user: User = {
          id: userData.Email || userData.UserName,
          name: `${userData.FirstName} ${userData.LastName}`.trim(),
          email: userData.Email,
          password: '',
          role: frontendRole,
          avatar: 'https://images.pexels.com/photos/1036623/pexels-photo-1036623.jpeg?auto=compress&cs=tinysrgb&w=200',
          createdAt: new Date().toISOString(),
        };
        
        setCurrentUser(user);
        localStorage.setItem('currentUser', JSON.stringify(user));
      } catch (error) {
        // User not authenticated, clear local storage
        localStorage.removeItem('currentUser');
      }
    };
    
    checkAuth();
  }, []);

  const fetchCars = async (filters?: { city?: string; minPrice?: number; maxPrice?: number; brand?: string; model?: string }) => {
    try {
      setLoading(true);
      const data = await apiService.getCars(filters);
      const mappedCars = data.map(mapCarDTOToCar);
      setCars(mappedCars);
    } catch (error) {
      console.error('Failed to fetch cars:', error);
      addToast('error', 'Failed to load cars');
    } finally {
      setLoading(false);
    }
  };

  const login = async (email: string, password: string): Promise<boolean> => {
    try {
      setLoading(true);
      const data = await apiService.login(email, password);
      
      // Map backend user data to frontend User type
      const backendRole = data.Role || 'User';
      const frontendRole = backendRole.toLowerCase() === 'owner' ? 'owner' : 'client';
      
      const user: User = {
        id: data.Email || data.UserName,
        name: `${data.FirstName} ${data.LastName}`.trim(),
        email: data.Email,
        password: '', // Don't store password
        role: frontendRole,
        avatar: 'https://images.pexels.com/photos/1036623/pexels-photo-1036623.jpeg?auto=compress&cs=tinysrgb&w=200',
        createdAt: new Date().toISOString(),
      };
      
      setCurrentUser(user);
      localStorage.setItem('currentUser', JSON.stringify(user));
      addToast('success', `Welcome back, ${user.name}!`);
      return true;
    } catch (error: any) {
      addToast('error', error.message || 'Login failed');
      return false;
    } finally {
      setLoading(false);
    }
  };

  const logout = () => {
    setCurrentUser(null);
    localStorage.removeItem('currentUser');
    document.cookie = 'access_token=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC;';
    addToast('info', 'You have been logged out');
  };

  const register = async (name: string, email: string, password: string, role: 'client' | 'owner'): Promise<boolean> => {
    try {
      setLoading(true);
      
      // Split name into first and last name
      const nameParts = name.trim().split(' ');
      const firstName = nameParts[0] || name;
      const lastName = nameParts.slice(1).join(' ') || '';
      
      const data = await apiService.register({
        email,
        password,
        userName: email,
        firstName,
        lastName,
        city: 'Unknown', // Default city, can be updated later
        role: role === 'client' ? 'User' : 'Owner',
      });

      const backendRole = data.Role || 'User';
      const frontendRole = backendRole.toLowerCase() === 'owner' ? 'owner' : 'client';

      const user: User = {
        id: data.Email || data.UserName,
        name: `${data.FirstName} ${data.LastName}`.trim(),
        email: data.Email,
        password: '',
        role: frontendRole,
        avatar: 'https://images.pexels.com/photos/1036623/pexels-photo-1036623.jpeg?auto=compress&cs=tinysrgb&w=200',
        createdAt: new Date().toISOString(),
      };

      setCurrentUser(user);
      localStorage.setItem('currentUser', JSON.stringify(user));
      addToast('success', 'Account created successfully!');
      return true;
    } catch (error: any) {
      addToast('error', error.message || 'Registration failed');
      return false;
    } finally {
      setLoading(false);
    }
  };

  const updateUser = async (updates: Partial<User>) => {
    if (!currentUser) return;

    try {
      setLoading(true);
      
      // Split name if it was updated
      let firstName = currentUser.name.split(' ')[0] || 'User';
      let lastName = currentUser.name.split(' ').slice(1).join(' ') || '';
      
      if (updates.name) {
        const nameParts = updates.name.trim().split(' ');
        firstName = nameParts[0] || 'User';
        lastName = nameParts.slice(1).join(' ') || '';
      }
      
      // Call backend API
      const data = await apiService.updateUser({
        firstName,
        lastName,
        city: updates.city || currentUser.city || 'Unknown',
      });
      
      // Update local state with backend response
      const backendRole = data.Role || 'User';
      const frontendRole = backendRole.toLowerCase() === 'owner' ? 'owner' : 'client';
      
      const updatedUser: User = {
        ...currentUser,
        name: `${data.FirstName} ${data.LastName}`.trim(),
        email: data.Email,
        role: frontendRole,
        ...updates,
      };
      
      setCurrentUser(updatedUser);
      localStorage.setItem('currentUser', JSON.stringify(updatedUser));
      addToast('success', 'Profile updated successfully');
    } catch (error: any) {
      addToast('error', error.message || 'Failed to update profile');
    } finally {
      setLoading(false);
    }
  };

  const addCar = async (carData: Omit<Car, 'id'>) => {
    try {
      setLoading(true);
      
      // Call backend API
      const data = await apiService.createCar({
        brand: carData.make,
        model: carData.model,
        year: carData.year,
        pricePerDay: carData.pricePerDay,
        city: carData.city,
        fuelType: carData.fuelType.charAt(0).toUpperCase() + carData.fuelType.slice(1),
        transmission: carData.transmission.charAt(0).toUpperCase() + carData.transmission.slice(1),
        description: carData.description,
        features: carData.features,
        imageUrl: carData.image,
        imageUrls: carData.images,
        seats: carData.seats,
      });
      
      // Map backend response to frontend Car type
      const newCar = mapCarDTOToCar(data);
      setCars([...cars, newCar]);
      addToast('success', 'Car added successfully');
    } catch (error: any) {
      addToast('error', error.message || 'Failed to add car');
    } finally {
      setLoading(false);
    }
  };

  const updateCar = (carId: string, updates: Partial<Car>) => {
    setCars(cars.map(car => car.id === carId ? { ...car, ...updates } : car));
    addToast('success', 'Car updated successfully');
  };

  const deleteCar = (carId: string) => {
    setCars(cars.filter(car => car.id !== carId));
    addToast('success', 'Car deleted successfully');
  };

  const createBooking = async (bookingData: Omit<Booking, 'id' | 'createdAt'>): Promise<string> => {
    try {
      setLoading(true);
      const data = await apiService.createBooking({
        carId: bookingData.carId,
        startDate: bookingData.startDate,
        endDate: bookingData.endDate,
      });
      
      // Map backend booking to frontend booking
      const newBooking: Booking = {
        id: data.Id,
        carId: data.CarId,
        clientId: bookingData.clientId,
        ownerId: bookingData.ownerId,
        startDate: data.StartDate,
        endDate: data.EndDate,
        totalPrice: data.TotalPrice,
        status: data.Status.toLowerCase() as 'pending' | 'accepted' | 'rejected' | 'completed' | 'cancelled',
        createdAt: new Date().toISOString(),
      };
      
      setBookings([...bookings, newBooking]);
      addToast('success', 'Booking request sent!');
      return newBooking.id;
    } catch (error: any) {
      addToast('error', error.message || 'Failed to create booking');
      throw error;
    } finally {
      setLoading(false);
    }
  };

  const updateBooking = (bookingId: string, updates: Partial<Booking>) => {
    setBookings(bookings.map(b => b.id === bookingId ? { ...b, ...updates } : b));

    if (updates.status === 'accepted') {
      addToast('success', 'Booking accepted!');
    } else if (updates.status === 'rejected') {
      addToast('error', 'Booking rejected');
    }
  };

  const cancelBooking = (bookingId: string) => {
    setBookings(bookings.map(b => b.id === bookingId ? { ...b, status: 'cancelled' as const } : b));
    addToast('info', 'Booking cancelled');
  };

  const addReview = (reviewData: Omit<Review, 'id' | 'createdAt'>) => {
    const newReview: Review = {
      ...reviewData,
      id: `review-${Date.now()}`,
      createdAt: new Date().toISOString(),
    };
    setReviews([...reviews, newReview]);

    const car = cars.find(c => c.id === reviewData.carId);
    if (car) {
      const carReviews = reviews.filter(r => r.carId === car.id);
      const totalRating = carReviews.reduce((sum, r) => sum + r.rating, 0) + reviewData.rating;
      const newRating = totalRating / (carReviews.length + 1);

      updateCar(car.id, {
        rating: Math.round(newRating * 10) / 10,
        reviewCount: car.reviewCount + 1,
      });
    }

    addToast('success', 'Review submitted successfully');
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
        loading,
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
        fetchCars,
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

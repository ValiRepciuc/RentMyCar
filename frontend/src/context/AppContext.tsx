import { createContext, useContext, useState, ReactNode, useEffect } from 'react';
import { User, Car, Booking, Review, Toast } from '../types';
import { users as initialUsers, cars as initialCars, bookings as initialBookings, reviews as initialReviews } from '../data/dummyData';

interface AppContextType {
  currentUser: User | null;
  users: User[];
  cars: Car[];
  bookings: Booking[];
  reviews: Review[];
  toasts: Toast[];
  login: (email: string, password: string) => boolean;
  logout: () => void;
  register: (name: string, email: string, password: string, role: 'client' | 'owner') => boolean;
  updateUser: (updates: Partial<User>) => void;
  addCar: (car: Omit<Car, 'id'>) => void;
  updateCar: (carId: string, updates: Partial<Car>) => void;
  deleteCar: (carId: string) => void;
  createBooking: (booking: Omit<Booking, 'id' | 'createdAt'>) => string;
  updateBooking: (bookingId: string, updates: Partial<Booking>) => void;
  cancelBooking: (bookingId: string) => void;
  addReview: (review: Omit<Review, 'id' | 'createdAt'>) => void;
  addToast: (type: Toast['type'], message: string) => void;
  removeToast: (id: string) => void;
}

const AppContext = createContext<AppContextType | undefined>(undefined);

export const AppProvider = ({ children }: { children: ReactNode }) => {
  const [currentUser, setCurrentUser] = useState<User | null>(null);
  const [users, setUsers] = useState<User[]>(initialUsers);
  const [cars, setCars] = useState<Car[]>(initialCars);
  const [bookings, setBookings] = useState<Booking[]>(initialBookings);
  const [reviews, setReviews] = useState<Review[]>(initialReviews);
  const [toasts, setToasts] = useState<Toast[]>([]);

  useEffect(() => {
    const savedUser = localStorage.getItem('currentUser');
    if (savedUser) {
      const user = JSON.parse(savedUser);
      const foundUser = users.find(u => u.id === user.id);
      if (foundUser) {
        setCurrentUser(foundUser);
      }
    }
  }, [users]);

  const login = (email: string, password: string): boolean => {
    const user = users.find(u => u.email === email && u.password === password);
    if (user) {
      setCurrentUser(user);
      localStorage.setItem('currentUser', JSON.stringify(user));
      addToast('success', `Welcome back, ${user.name}!`);
      return true;
    }
    addToast('error', 'Invalid email or password');
    return false;
  };

  const logout = () => {
    setCurrentUser(null);
    localStorage.removeItem('currentUser');
    addToast('info', 'You have been logged out');
  };

  const register = (name: string, email: string, password: string, role: 'client' | 'owner'): boolean => {
    if (users.find(u => u.email === email)) {
      addToast('error', 'Email already exists');
      return false;
    }

    const newUser: User = {
      id: `user-${Date.now()}`,
      name,
      email,
      password,
      role,
      avatar: 'https://images.pexels.com/photos/1036623/pexels-photo-1036623.jpeg?auto=compress&cs=tinysrgb&w=200',
      createdAt: new Date().toISOString(),
    };

    setUsers([...users, newUser]);
    setCurrentUser(newUser);
    localStorage.setItem('currentUser', JSON.stringify(newUser));
    addToast('success', 'Account created successfully!');
    return true;
  };

  const updateUser = (updates: Partial<User>) => {
    if (!currentUser) return;

    const updatedUser = { ...currentUser, ...updates };
    setCurrentUser(updatedUser);
    setUsers(users.map(u => u.id === currentUser.id ? updatedUser : u));
    localStorage.setItem('currentUser', JSON.stringify(updatedUser));
    addToast('success', 'Profile updated successfully');
  };

  const addCar = (carData: Omit<Car, 'id'>) => {
    const newCar: Car = {
      ...carData,
      id: `car-${Date.now()}`,
    };
    setCars([...cars, newCar]);
    addToast('success', 'Car added successfully');
  };

  const updateCar = (carId: string, updates: Partial<Car>) => {
    setCars(cars.map(car => car.id === carId ? { ...car, ...updates } : car));
    addToast('success', 'Car updated successfully');
  };

  const deleteCar = (carId: string) => {
    setCars(cars.filter(car => car.id !== carId));
    addToast('success', 'Car deleted successfully');
  };

  const createBooking = (bookingData: Omit<Booking, 'id' | 'createdAt'>): string => {
    const newBooking: Booking = {
      ...bookingData,
      id: `booking-${Date.now()}`,
      createdAt: new Date().toISOString(),
    };
    setBookings([...bookings, newBooking]);
    addToast('success', 'Booking request sent!');
    return newBooking.id;
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

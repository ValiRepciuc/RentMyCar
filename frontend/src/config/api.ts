const API_BASE_URL = 'http://localhost:5039';

export const apiConfig = {
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  credentials: 'include' as RequestCredentials, // For JWT cookies
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

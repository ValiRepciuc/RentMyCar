import { useState } from 'react';
import { Calendar } from 'lucide-react';
import { useApp } from '../context/AppContext';
import { BookingCard } from '../components/BookingCard';
import { EmptyState } from '../components/Loading';

export const ManageBookings = () => {
  const { currentUser, bookings, cars, users, updateBooking } = useApp();
  const [filter, setFilter] = useState<'all' | 'pending' | 'accepted' | 'completed'>('all');

  if (!currentUser) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <p className="text-gray-600">Please login to view bookings</p>
      </div>
    );
  }

  const ownerBookings = bookings.filter(b => b.ownerId === currentUser.id);
  const filteredBookings = filter === 'all'
    ? ownerBookings
    : ownerBookings.filter(b => b.status === filter);

  const handleAccept = async (bookingId: string) => {
    await updateBooking(bookingId, { status: 'accepted' });
  };

  const handleReject = async (bookingId: string) => {
    await updateBooking(bookingId, { status: 'rejected' });
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900 mb-2">Manage Bookings</h1>
          <p className="text-gray-600">
            Review and manage booking requests for your cars
          </p>
        </div>

        <div className="mb-6 flex flex-wrap gap-3">
          <button
            onClick={() => setFilter('all')}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              filter === 'all'
                ? 'bg-blue-600 text-white'
                : 'bg-white text-gray-700 hover:bg-gray-50'
            }`}
          >
            All ({ownerBookings.length})
          </button>
          <button
            onClick={() => setFilter('pending')}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              filter === 'pending'
                ? 'bg-blue-600 text-white'
                : 'bg-white text-gray-700 hover:bg-gray-50'
            }`}
          >
            Pending ({ownerBookings.filter(b => b.status === 'pending').length})
          </button>
          <button
            onClick={() => setFilter('accepted')}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              filter === 'accepted'
                ? 'bg-blue-600 text-white'
                : 'bg-white text-gray-700 hover:bg-gray-50'
            }`}
          >
            Accepted ({ownerBookings.filter(b => b.status === 'accepted').length})
          </button>
          <button
            onClick={() => setFilter('completed')}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              filter === 'completed'
                ? 'bg-blue-600 text-white'
                : 'bg-white text-gray-700 hover:bg-gray-50'
            }`}
          >
            Completed ({ownerBookings.filter(b => b.status === 'completed').length})
          </button>
        </div>

        {filteredBookings.length === 0 ? (
          <EmptyState
            message={`You have no ${filter === 'all' ? '' : filter} bookings yet.`}
            icon={<Calendar className="w-16 h-16 text-gray-400" />}
          />
        ) : (
          <div className="space-y-4">
            {filteredBookings.map(booking => {
              const car = cars.find(c => c.id === booking.carId);
              const client = users.find(u => u.id === booking.clientId);

              if (!car || !client) return null;

              return (
                <BookingCard
                  key={booking.id}
                  booking={booking}
                  car={car}
                  user={client}
                  type="owner"
                  onAccept={() => handleAccept(booking.id)}
                  onReject={() => handleReject(booking.id)}
                />
              );
            })}
          </div>
        )}
      </div>
    </div>
  );
};

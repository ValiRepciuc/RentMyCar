import { useState } from 'react';
import { Calendar } from 'lucide-react';
import { useApp } from '../context/AppContext';
import { BookingCard } from '../components/BookingCard';
import { ReviewModal } from '../components/ReviewModal';
import { ConfirmModal } from '../components/ConfirmModal';
import { EmptyState } from '../components/Loading';

interface MyBookingsProps {
  onNavigate: (page: string) => void;
}

export const MyBookings = ({ onNavigate }: MyBookingsProps) => {
  const { currentUser, bookings, cars, users, cancelBooking, addReview } = useApp();
  const [selectedBooking, setSelectedBooking] = useState<string | null>(null);
  const [showReviewModal, setShowReviewModal] = useState(false);
  const [showCancelModal, setShowCancelModal] = useState(false);
  const [filter, setFilter] = useState<'all' | 'pending' | 'accepted' | 'completed'>('all');

  if (!currentUser) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <p className="text-gray-600">Please login to view your bookings</p>
      </div>
    );
  }

  const myBookings = bookings.filter(b => b.clientId === currentUser.id);
  const filteredBookings = filter === 'all'
    ? myBookings
    : myBookings.filter(b => b.status === filter);

  const handleCancelBooking = async () => {
    if (selectedBooking) {
      await cancelBooking(selectedBooking);
      setShowCancelModal(false);
      setSelectedBooking(null);
    }
  };

  const handleAddReview = async (rating: number, comment: string) => {
    if (!selectedBooking) return;

    const booking = bookings.find(b => b.id === selectedBooking);
    if (!booking) return;

    await addReview({
      bookingId: booking.id,
      carId: booking.carId,
      clientId: currentUser.id,
      rating,
      comment,
    });
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900 mb-2">My Bookings</h1>
          <p className="text-gray-600">
            Manage your car rental bookings
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
            All ({myBookings.length})
          </button>
          <button
            onClick={() => setFilter('pending')}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              filter === 'pending'
                ? 'bg-blue-600 text-white'
                : 'bg-white text-gray-700 hover:bg-gray-50'
            }`}
          >
            Pending ({myBookings.filter(b => b.status === 'pending').length})
          </button>
          <button
            onClick={() => setFilter('accepted')}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              filter === 'accepted'
                ? 'bg-blue-600 text-white'
                : 'bg-white text-gray-700 hover:bg-gray-50'
            }`}
          >
            Accepted ({myBookings.filter(b => b.status === 'accepted').length})
          </button>
          <button
            onClick={() => setFilter('completed')}
            className={`px-4 py-2 rounded-lg font-medium transition-colors ${
              filter === 'completed'
                ? 'bg-blue-600 text-white'
                : 'bg-white text-gray-700 hover:bg-gray-50'
            }`}
          >
            Completed ({myBookings.filter(b => b.status === 'completed').length})
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
              const owner = users.find(u => u.id === booking.ownerId);

              if (!car || !owner) return null;

              return (
                <BookingCard
                  key={booking.id}
                  booking={booking}
                  car={car}
                  user={owner}
                  type="client"
                  onCancel={() => {
                    setSelectedBooking(booking.id);
                    setShowCancelModal(true);
                  }}
                  onReview={() => {
                    setSelectedBooking(booking.id);
                    setShowReviewModal(true);
                  }}
                />
              );
            })}
          </div>
        )}
      </div>

      {showReviewModal && selectedBooking && (
        <ReviewModal
          carName={`${cars.find(c => c.id === bookings.find(b => b.id === selectedBooking)?.carId)?.make} ${cars.find(c => c.id === bookings.find(b => b.id === selectedBooking)?.carId)?.model}`}
          onClose={() => {
            setShowReviewModal(false);
            setSelectedBooking(null);
          }}
          onSubmit={handleAddReview}
        />
      )}

      {showCancelModal && (
        <ConfirmModal
          title="Cancel Booking"
          message="Are you sure you want to cancel this booking? This action cannot be undone."
          confirmText="Yes, Cancel"
          cancelText="No, Keep It"
          danger
          onConfirm={handleCancelBooking}
          onCancel={() => {
            setShowCancelModal(false);
            setSelectedBooking(null);
          }}
        />
      )}
    </div>
  );
};

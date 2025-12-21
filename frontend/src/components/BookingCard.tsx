import { Calendar, DollarSign, CheckCircle, XCircle, Clock, Ban } from 'lucide-react';
import { Booking, Car, User } from '../types';

interface BookingCardProps {
  booking: Booking;
  car: Car;
  user: User;
  type: 'client' | 'owner';
  onAccept?: () => void;
  onReject?: () => void;
  onCancel?: () => void;
  onReview?: () => void;
}

export const BookingCard = ({
  booking,
  car,
  user,
  type,
  onAccept,
  onReject,
  onCancel,
  onReview,
}: BookingCardProps) => {
  const statusColors = {
    pending: 'bg-yellow-100 text-yellow-800 border-yellow-300',
    accepted: 'bg-green-100 text-green-800 border-green-300',
    rejected: 'bg-red-100 text-red-800 border-red-300',
    completed: 'bg-blue-100 text-blue-800 border-blue-300',
    cancelled: 'bg-gray-100 text-gray-800 border-gray-300',
  };

  const statusIcons = {
    pending: <Clock className="w-4 h-4" />,
    accepted: <CheckCircle className="w-4 h-4" />,
    rejected: <XCircle className="w-4 h-4" />,
    completed: <CheckCircle className="w-4 h-4" />,
    cancelled: <Ban className="w-4 h-4" />,
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric',
    });
  };

  const calculateDays = () => {
    const start = new Date(booking.startDate);
    const end = new Date(booking.endDate);
    const days = Math.ceil((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24));
    return days;
  };

  return (
    <div className="bg-white rounded-xl shadow-md hover:shadow-lg transition-shadow p-5">
      <div className="flex flex-col sm:flex-row gap-4">
        <img
          src={car.image}
          alt={`${car.make} ${car.model}`}
          className="w-full sm:w-32 h-32 object-cover rounded-lg"
        />

        <div className="flex-1">
          <div className="flex items-start justify-between mb-3">
            <div>
              <h3 className="text-lg font-bold text-gray-900">
                {car.make} {car.model}
              </h3>
              <p className="text-sm text-gray-600">
                {type === 'client' ? 'Owner' : 'Renter'}: {user.name}
              </p>
            </div>
            <span
              className={`flex items-center gap-1 px-3 py-1 rounded-full text-xs font-semibold border ${statusColors[booking.status]}`}
            >
              {statusIcons[booking.status]}
              {booking.status.charAt(0).toUpperCase() + booking.status.slice(1)}
            </span>
          </div>

          <div className="space-y-2 mb-4">
            <div className="flex items-center gap-2 text-sm text-gray-600">
              <Calendar className="w-4 h-4" />
              <span>
                {formatDate(booking.startDate)} - {formatDate(booking.endDate)}
              </span>
              <span className="text-gray-400">({calculateDays()} days)</span>
            </div>
            <div className="flex items-center gap-2 text-sm text-gray-600">
              <DollarSign className="w-4 h-4" />
              <span className="font-semibold text-gray-900">
                ${booking.totalPrice}
              </span>
              <span>total</span>
            </div>
          </div>

          <div className="flex flex-wrap gap-2">
            {type === 'owner' && booking.status === 'pending' && (
              <>
                <button
                  onClick={onAccept}
                  className="px-4 py-2 bg-green-600 text-white text-sm font-medium rounded-lg hover:bg-green-700 transition-colors"
                >
                  Accept
                </button>
                <button
                  onClick={onReject}
                  className="px-4 py-2 bg-red-600 text-white text-sm font-medium rounded-lg hover:bg-red-700 transition-colors"
                >
                  Reject
                </button>
              </>
            )}

            {type === 'client' && booking.status === 'pending' && (
              <button
                onClick={onCancel}
                className="px-4 py-2 bg-gray-600 text-white text-sm font-medium rounded-lg hover:bg-gray-700 transition-colors"
              >
                Cancel Booking
              </button>
            )}

            {type === 'client' && booking.status === 'completed' && onReview && (
              <button
                onClick={onReview}
                className="px-4 py-2 bg-blue-600 text-white text-sm font-medium rounded-lg hover:bg-blue-700 transition-colors"
              >
                Leave Review
              </button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

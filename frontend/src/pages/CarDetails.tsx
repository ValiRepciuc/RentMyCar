import { useState } from 'react';
import { MapPin, Star, Fuel, Users, Calendar, Settings, ArrowLeft } from 'lucide-react';
import { useApp } from '../context/AppContext';
import { BookingModal } from '../components/BookingModal';

interface CarDetailsProps {
  carId: string;
  onNavigate: (page: string) => void;
}

export const CarDetails = ({ carId, onNavigate }: CarDetailsProps) => {
  const { cars, users, reviews, currentUser, createBooking } = useApp();
  const [showBookingModal, setShowBookingModal] = useState(false);

  const car = cars.find(c => c.id === carId);
  const owner = car ? users.find(u => u.id === car.ownerId) : null;
  const carReviews = reviews.filter(r => r.carId === carId);

  if (!car || !owner) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <p className="text-gray-600">Car not found</p>
      </div>
    );
  }

  const handleBook = async (startDate: string, endDate: string) => {
    if (!currentUser) {
      onNavigate('login');
      return;
    }

    const start = new Date(startDate);
    const end = new Date(endDate);
    const days = Math.ceil((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24));
    const totalPrice = days * car.pricePerDay;

    await createBooking({
      carId: car.id,
      clientId: currentUser.id,
      ownerId: car.ownerId,
      startDate,
      endDate,
      totalPrice,
      status: 'pending',
    });
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <button
          onClick={() => onNavigate('cars')}
          className="flex items-center gap-2 text-gray-600 hover:text-gray-900 mb-6 font-medium"
        >
          <ArrowLeft className="w-5 h-5" />
          Back to listings
        </button>

        <div className="bg-white rounded-2xl shadow-lg overflow-hidden">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 p-8">
            <div>
              <img
                src={car.image}
                alt={`${car.make} ${car.model}`}
                className="w-full h-96 object-cover rounded-xl"
              />
              <div className="grid grid-cols-3 gap-4 mt-4">
                {car.images.slice(0, 3).map((img, idx) => (
                  <img
                    key={idx}
                    src={img}
                    alt={`${car.make} ${car.model} ${idx + 1}`}
                    className="w-full h-24 object-cover rounded-lg"
                  />
                ))}
              </div>
            </div>

            <div>
              <div className="flex items-start justify-between mb-4">
                <div>
                  <h1 className="text-4xl font-bold text-gray-900 mb-2">
                    {car.make} {car.model}
                  </h1>
                  <p className="text-xl text-gray-600">{car.year}</p>
                </div>
                <div className="bg-yellow-50 px-4 py-2 rounded-lg flex items-center gap-2">
                  <Star className="w-6 h-6 text-yellow-500 fill-current" />
                  <span className="text-2xl font-bold">{car.rating}</span>
                  <span className="text-gray-500">({car.reviewCount})</span>
                </div>
              </div>

              <div className="flex items-center gap-6 text-gray-600 mb-6 pb-6 border-b border-gray-200">
                <div className="flex items-center gap-2">
                  <MapPin className="w-5 h-5" />
                  <span>{car.city}</span>
                </div>
                <div className="flex items-center gap-2">
                  <Users className="w-5 h-5" />
                  <span>{car.seats} seats</span>
                </div>
                <div className="flex items-center gap-2">
                  <Fuel className="w-5 h-5" />
                  <span className="capitalize">{car.fuelType}</span>
                </div>
                <div className="flex items-center gap-2">
                  <Settings className="w-5 h-5" />
                  <span className="capitalize">{car.transmission}</span>
                </div>
              </div>

              <div className="mb-6">
                <h3 className="text-lg font-bold text-gray-900 mb-3">About this car</h3>
                <p className="text-gray-600 leading-relaxed">{car.description}</p>
              </div>

              <div className="mb-6">
                <h3 className="text-lg font-bold text-gray-900 mb-3">Features</h3>
                <div className="flex flex-wrap gap-2">
                  {car.features.map((feature, idx) => (
                    <span
                      key={idx}
                      className="px-3 py-2 bg-blue-50 text-blue-700 rounded-lg text-sm font-medium"
                    >
                      {feature}
                    </span>
                  ))}
                </div>
              </div>

              <div className="mb-6 p-4 bg-gray-50 rounded-lg">
                <div className="flex items-center gap-3 mb-2">
                  <img
                    src={owner.avatar}
                    alt={owner.name}
                    className="w-12 h-12 rounded-full object-cover"
                  />
                  <div>
                    <p className="font-semibold text-gray-900">Hosted by {owner.name}</p>
                    <p className="text-sm text-gray-600">{owner.email}</p>
                  </div>
                </div>
              </div>

              <div className="flex items-center justify-between p-6 bg-blue-50 rounded-xl">
                <div>
                  <p className="text-gray-600 mb-1">Price per day</p>
                  <p className="text-4xl font-bold text-blue-600">${car.pricePerDay}</p>
                </div>
                {car.available ? (
                  <button
                    onClick={() => setShowBookingModal(true)}
                    className="px-8 py-4 bg-blue-600 text-white font-semibold rounded-lg hover:bg-blue-700 transition-colors shadow-lg flex items-center gap-2"
                  >
                    <Calendar className="w-5 h-5" />
                    Book Now
                  </button>
                ) : (
                  <button
                    disabled
                    className="px-8 py-4 bg-gray-300 text-gray-600 font-semibold rounded-lg cursor-not-allowed"
                  >
                    Not Available
                  </button>
                )}
              </div>
            </div>
          </div>

          {carReviews.length > 0 && (
            <div className="p-8 border-t border-gray-200">
              <h3 className="text-2xl font-bold text-gray-900 mb-6">Reviews</h3>
              <div className="space-y-4">
                {carReviews.map(review => {
                  const reviewer = users.find(u => u.id === review.clientId);
                  return (
                    <div key={review.id} className="bg-gray-50 p-4 rounded-lg">
                      <div className="flex items-center gap-3 mb-3">
                        <img
                          src={reviewer?.avatar}
                          alt={reviewer?.name}
                          className="w-10 h-10 rounded-full object-cover"
                        />
                        <div className="flex-1">
                          <p className="font-semibold text-gray-900">{reviewer?.name}</p>
                          <div className="flex items-center gap-1">
                            {Array.from({ length: review.rating }).map((_, i) => (
                              <Star key={i} className="w-4 h-4 text-yellow-500 fill-current" />
                            ))}
                          </div>
                        </div>
                        <p className="text-sm text-gray-500">
                          {new Date(review.createdAt).toLocaleDateString()}
                        </p>
                      </div>
                      <p className="text-gray-700">{review.comment}</p>
                    </div>
                  );
                })}
              </div>
            </div>
          )}
        </div>
      </div>

      {showBookingModal && (
        <BookingModal
          car={car}
          onClose={() => setShowBookingModal(false)}
          onBook={handleBook}
        />
      )}
    </div>
  );
};

import { MapPin, Star, Fuel, Users } from 'lucide-react';
import { Car } from '../types';

interface CarCardProps {
  car: Car;
  onClick: () => void;
}

export const CarCard = ({ car, onClick }: CarCardProps) => {
  return (
    <div
      onClick={onClick}
      className="bg-white rounded-xl shadow-md hover:shadow-xl transition-all duration-300 cursor-pointer overflow-hidden group"
    >
      <div className="relative h-48 overflow-hidden">
        <img
          src={car.image}
          alt={`${car.make} ${car.model}`}
          className="w-full h-full object-cover group-hover:scale-110 transition-transform duration-300"
        />
        {!car.available && (
          <div className="absolute inset-0 bg-black bg-opacity-50 flex items-center justify-center">
            <span className="bg-red-600 text-white px-4 py-2 rounded-lg font-semibold">
              Not Available
            </span>
          </div>
        )}
        <div className="absolute top-3 right-3 bg-white px-3 py-1 rounded-full shadow-md flex items-center gap-1">
          <Star className="w-4 h-4 text-yellow-500 fill-current" />
          <span className="font-semibold text-sm">{car.rating}</span>
          <span className="text-gray-500 text-xs">({car.reviewCount})</span>
        </div>
      </div>

      <div className="p-4">
        <div className="flex items-start justify-between mb-2">
          <div>
            <h3 className="text-lg font-bold text-gray-900">
              {car.make} {car.model}
            </h3>
            <p className="text-sm text-gray-500">{car.year}</p>
          </div>
          <div className="text-right">
            <p className="text-2xl font-bold text-blue-600">${car.pricePerDay}</p>
            <p className="text-xs text-gray-500">per day</p>
          </div>
        </div>

        <div className="flex items-center gap-4 text-sm text-gray-600 mb-3">
          <div className="flex items-center gap-1">
            <MapPin className="w-4 h-4" />
            <span>{car.city}</span>
          </div>
          <div className="flex items-center gap-1">
            <Users className="w-4 h-4" />
            <span>{car.seats} seats</span>
          </div>
          <div className="flex items-center gap-1">
            <Fuel className="w-4 h-4" />
            <span className="capitalize">{car.fuelType}</span>
          </div>
        </div>

        <div className="flex flex-wrap gap-2">
          {car.features.slice(0, 2).map((feature, index) => (
            <span
              key={index}
              className="text-xs bg-blue-50 text-blue-700 px-2 py-1 rounded-full"
            >
              {feature}
            </span>
          ))}
          {car.features.length > 2 && (
            <span className="text-xs bg-gray-100 text-gray-700 px-2 py-1 rounded-full">
              +{car.features.length - 2} more
            </span>
          )}
        </div>
      </div>
    </div>
  );
};

import { useState } from 'react';
import { Filter } from 'lucide-react';
import { useApp } from '../context/AppContext';
import { CarCard } from '../components/CarCard';
import { FilterSidebar, Filters } from '../components/FilterSidebar';
import { EmptyState } from '../components/Loading';
import { Car as CarIcon } from 'lucide-react';

interface CarListingsProps {
  onNavigate: (page: string, carId?: string) => void;
}

export const CarListings = ({ onNavigate }: CarListingsProps) => {
  const { cars } = useApp();
  const [filters, setFilters] = useState<Filters>({
    city: '',
    minPrice: 0,
    maxPrice: 200,
    model: '',
    sortBy: '',
  });
  const [showMobileFilters, setShowMobileFilters] = useState(false);

  const cities = Array.from(new Set(cars.map(car => car.city)));

  const filteredCars = cars
    .filter(car => {
      if (filters.city && car.city !== filters.city) return false;
      if (car.pricePerDay < filters.minPrice || car.pricePerDay > filters.maxPrice) return false;
      if (filters.model && !car.model.toLowerCase().includes(filters.model.toLowerCase())) return false;
      return true;
    })
    .sort((a, b) => {
      switch (filters.sortBy) {
        case 'price-asc':
          return a.pricePerDay - b.pricePerDay;
        case 'price-desc':
          return b.pricePerDay - a.pricePerDay;
        case 'year-asc':
          return a.year - b.year;
        case 'year-desc':
          return b.year - a.year;
        default:
          return 0;
      }
    });

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900 mb-2">Available Cars</h1>
          <p className="text-gray-600">
            Browse through {cars.length} cars available for rent
          </p>
        </div>

        <button
          onClick={() => setShowMobileFilters(true)}
          className="lg:hidden fixed bottom-6 right-6 bg-blue-600 text-white p-4 rounded-full shadow-lg hover:bg-blue-700 transition-colors z-40 flex items-center gap-2"
        >
          <Filter className="w-5 h-5" />
          <span className="font-semibold">Filters</span>
        </button>

        <div className="flex flex-col lg:flex-row gap-8">
          <aside className="hidden lg:block w-64 flex-shrink-0">
            <FilterSidebar cities={cities} onFilterChange={setFilters} />
          </aside>

          {showMobileFilters && (
            <FilterSidebar
              cities={cities}
              onFilterChange={setFilters}
              onClose={() => setShowMobileFilters(false)}
              isMobile
            />
          )}

          <div className="flex-1">
            {filteredCars.length === 0 ? (
              <EmptyState
                message="No cars match your filters. Try adjusting your search criteria."
                icon={<CarIcon className="w-16 h-16 text-gray-400" />}
              />
            ) : (
              <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-6">
                {filteredCars.map(car => (
                  <CarCard
                    key={car.id}
                    car={car}
                    onClick={() => onNavigate('car-details', car.id)}
                  />
                ))}
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

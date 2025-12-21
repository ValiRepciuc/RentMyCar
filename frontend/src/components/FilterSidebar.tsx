import { X } from 'lucide-react';
import { useState } from 'react';

interface FilterSidebarProps {
  cities: string[];
  onFilterChange: (filters: Filters) => void;
  onClose?: () => void;
  isMobile?: boolean;
}

export interface Filters {
  city: string;
  minPrice: number;
  maxPrice: number;
  model: string;
  sortBy: 'price-asc' | 'price-desc' | 'year-asc' | 'year-desc' | '';
}

export const FilterSidebar = ({ cities, onFilterChange, onClose, isMobile }: FilterSidebarProps) => {
  const [filters, setFilters] = useState<Filters>({
    city: '',
    minPrice: 0,
    maxPrice: 200,
    model: '',
    sortBy: '',
  });

  const handleChange = (key: keyof Filters, value: string | number) => {
    const newFilters = { ...filters, [key]: value };
    setFilters(newFilters);
    onFilterChange(newFilters);
  };

  const handleReset = () => {
    const resetFilters: Filters = {
      city: '',
      minPrice: 0,
      maxPrice: 200,
      model: '',
      sortBy: '',
    };
    setFilters(resetFilters);
    onFilterChange(resetFilters);
  };

  const content = (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h3 className="text-lg font-bold text-gray-900">Filters</h3>
        {isMobile && (
          <button onClick={onClose} className="text-gray-500 hover:text-gray-700">
            <X className="w-6 h-6" />
          </button>
        )}
      </div>

      <div>
        <label className="block text-sm font-semibold text-gray-700 mb-2">City</label>
        <select
          value={filters.city}
          onChange={(e) => handleChange('city', e.target.value)}
          className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
        >
          <option value="">All Cities</option>
          {cities.map((city) => (
            <option key={city} value={city}>
              {city}
            </option>
          ))}
        </select>
      </div>

      <div>
        <label className="block text-sm font-semibold text-gray-700 mb-2">
          Price Range: ${filters.minPrice} - ${filters.maxPrice}
        </label>
        <div className="space-y-2">
          <input
            type="range"
            min="0"
            max="200"
            value={filters.minPrice}
            onChange={(e) => handleChange('minPrice', Number(e.target.value))}
            className="w-full"
          />
          <input
            type="range"
            min="0"
            max="200"
            value={filters.maxPrice}
            onChange={(e) => handleChange('maxPrice', Number(e.target.value))}
            className="w-full"
          />
        </div>
      </div>

      <div>
        <label className="block text-sm font-semibold text-gray-700 mb-2">Search Model</label>
        <input
          type="text"
          placeholder="e.g., Camry, Model 3..."
          value={filters.model}
          onChange={(e) => handleChange('model', e.target.value)}
          className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
        />
      </div>

      <div>
        <label className="block text-sm font-semibold text-gray-700 mb-2">Sort By</label>
        <select
          value={filters.sortBy}
          onChange={(e) => handleChange('sortBy', e.target.value)}
          className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
        >
          <option value="">Default</option>
          <option value="price-asc">Price: Low to High</option>
          <option value="price-desc">Price: High to Low</option>
          <option value="year-asc">Year: Oldest First</option>
          <option value="year-desc">Year: Newest First</option>
        </select>
      </div>

      <button
        onClick={handleReset}
        className="w-full px-4 py-2 bg-gray-200 text-gray-700 font-medium rounded-lg hover:bg-gray-300 transition-colors"
      >
        Reset Filters
      </button>
    </div>
  );

  if (isMobile) {
    return (
      <div className="fixed inset-0 bg-black bg-opacity-50 z-50 flex items-end">
        <div className="bg-white rounded-t-2xl p-6 w-full max-h-[80vh] overflow-y-auto">
          {content}
        </div>
      </div>
    );
  }

  return (
    <div className="bg-white rounded-xl shadow-md p-6 sticky top-20">
      {content}
    </div>
  );
};

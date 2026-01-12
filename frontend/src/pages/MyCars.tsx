import { useState } from 'react';
import { Plus, Edit, Trash2, Car as CarIcon } from 'lucide-react';
import { useApp } from '../context/AppContext';
import { CarCard } from '../components/CarCard';
import { CarFormModal } from '../components/CarFormModal';
import { ConfirmModal } from '../components/ConfirmModal';
import { EmptyState } from '../components/Loading';
import { Car } from '../types';

interface MyCarsProps {
  onNavigate: (page: string, carId?: string) => void;
}

export const MyCars = ({ onNavigate }: MyCarsProps) => {
  const { currentUser, cars, addCar, updateCar, deleteCar } = useApp();
  const [showCarModal, setShowCarModal] = useState(false);
  const [selectedCar, setSelectedCar] = useState<Car | null>(null);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [carToDelete, setCarToDelete] = useState<string | null>(null);

  if (!currentUser) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <p className="text-gray-600">Please login to view your cars</p>
      </div>
    );
  }

  const myCars = cars.filter(c => c.ownerId === currentUser.id);

  const handleAddCar = async (carData: Omit<Car, 'id' | 'ownerId' | 'rating' | 'reviewCount'>) => {
    await addCar({
      ...carData,
      ownerId: currentUser.id,
      rating: 0,
      reviewCount: 0,
    });
  };

  const handleUpdateCar = (carData: Omit<Car, 'id' | 'ownerId' | 'rating' | 'reviewCount'>) => {
    if (selectedCar) {
      updateCar(selectedCar.id, carData);
    }
  };

  const handleDeleteCar = () => {
    if (carToDelete) {
      deleteCar(carToDelete);
      setShowDeleteModal(false);
      setCarToDelete(null);
    }
  };

  const openEditModal = (car: Car, e: React.MouseEvent) => {
    e.stopPropagation();
    setSelectedCar(car);
    setShowCarModal(true);
  };

  const openDeleteModal = (carId: string, e: React.MouseEvent) => {
    e.stopPropagation();
    setCarToDelete(carId);
    setShowDeleteModal(true);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="flex items-center justify-between mb-8">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 mb-2">My Cars</h1>
            <p className="text-gray-600">
              Manage your car listings
            </p>
          </div>
          <button
            onClick={() => {
              setSelectedCar(null);
              setShowCarModal(true);
            }}
            className="px-4 py-3 bg-blue-600 text-white font-semibold rounded-lg hover:bg-blue-700 transition-colors flex items-center gap-2 shadow-lg"
          >
            <Plus className="w-5 h-5" />
            Add New Car
          </button>
        </div>

        {myCars.length === 0 ? (
          <EmptyState
            message="You haven't listed any cars yet. Start earning by adding your first car!"
            icon={<CarIcon className="w-16 h-16 text-gray-400" />}
          />
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-6">
            {myCars.map(car => (
              <div key={car.id} className="relative group">
                <CarCard
                  car={car}
                  onClick={() => onNavigate('car-details', car.id)}
                />
                <div className="absolute top-4 left-4 flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity z-10">
                  <button
                    onClick={(e) => openEditModal(car, e)}
                    className="p-2 bg-white rounded-lg shadow-lg hover:bg-gray-100 transition-colors"
                    title="Edit car"
                  >
                    <Edit className="w-4 h-4 text-blue-600" />
                  </button>
                  <button
                    onClick={(e) => openDeleteModal(car.id, e)}
                    className="p-2 bg-white rounded-lg shadow-lg hover:bg-gray-100 transition-colors"
                    title="Delete car"
                  >
                    <Trash2 className="w-4 h-4 text-red-600" />
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>

      {showCarModal && (
        <CarFormModal
          car={selectedCar || undefined}
          onClose={() => {
            setShowCarModal(false);
            setSelectedCar(null);
          }}
          onSave={selectedCar ? handleUpdateCar : handleAddCar}
        />
      )}

      {showDeleteModal && (
        <ConfirmModal
          title="Delete Car"
          message="Are you sure you want to delete this car? This action cannot be undone."
          confirmText="Yes, Delete"
          cancelText="Cancel"
          danger
          onConfirm={handleDeleteCar}
          onCancel={() => {
            setShowDeleteModal(false);
            setCarToDelete(null);
          }}
        />
      )}
    </div>
  );
};

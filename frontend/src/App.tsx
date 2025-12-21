import { useState } from 'react';
import { Navbar } from './components/Navbar';
import { ToastContainer } from './components/Toast';
import { Home } from './pages/Home';
import { Login } from './pages/Login';
import { Register } from './pages/Register';
import { CarListings } from './pages/CarListings';
import { CarDetails } from './pages/CarDetails';
import { Profile } from './pages/Profile';
import { MyBookings } from './pages/MyBookings';
import { MyCars } from './pages/MyCars';
import { ManageBookings } from './pages/ManageBookings';

type Page = 'home' | 'login' | 'register' | 'cars' | 'car-details' | 'profile' | 'my-bookings' | 'my-cars' | 'manage-bookings';

function App() {
  const [currentPage, setCurrentPage] = useState<Page>('home');
  const [selectedCarId, setSelectedCarId] = useState<string | undefined>();

  const handleNavigate = (page: string, carId?: string) => {
    setCurrentPage(page as Page);
    if (carId) {
      setSelectedCarId(carId);
    }
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  const renderPage = () => {
    switch (currentPage) {
      case 'home':
        return <Home onNavigate={handleNavigate} />;
      case 'login':
        return <Login onNavigate={handleNavigate} />;
      case 'register':
        return <Register onNavigate={handleNavigate} />;
      case 'cars':
        return <CarListings onNavigate={handleNavigate} />;
      case 'car-details':
        return selectedCarId ? (
          <CarDetails carId={selectedCarId} onNavigate={handleNavigate} />
        ) : (
          <CarListings onNavigate={handleNavigate} />
        );
      case 'profile':
        return <Profile />;
      case 'my-bookings':
        return <MyBookings onNavigate={handleNavigate} />;
      case 'my-cars':
        return <MyCars onNavigate={handleNavigate} />;
      case 'manage-bookings':
        return <ManageBookings />;
      default:
        return <Home onNavigate={handleNavigate} />;
    }
  };

  return (
    <>
      <Navbar currentPage={currentPage} onNavigate={handleNavigate} />
      <ToastContainer />
      {renderPage()}
    </>
  );
}

export default App;

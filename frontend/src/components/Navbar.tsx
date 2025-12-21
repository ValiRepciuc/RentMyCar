import { Car, User, LogOut, Menu, X } from "lucide-react";
import { useApp } from "../context/AppContext";
import { useState } from "react";

interface NavbarProps {
  currentPage: string;
  onNavigate: (page: string) => void;
}

export const Navbar = ({ currentPage, onNavigate }: NavbarProps) => {
  const { currentUser, logout } = useApp();
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false);

  const handleLogout = () => {
    logout();
    onNavigate("home");
    setMobileMenuOpen(false);
  };

  const navigate = (page: string) => {
    onNavigate(page);
    setMobileMenuOpen(false);
  };

  return (
    <nav className="bg-white shadow-md sticky top-0 z-40">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          <button
            onClick={() => navigate("home")}
            className="flex items-center gap-2 text-xl font-bold text-blue-600 hover:text-blue-700 transition-colors"
          >
            <Car className="w-8 h-8" />
            <span>RentMyCar</span>
          </button>

          <div className="hidden md:flex items-center gap-6">
            {currentUser ? (
              <>
                <button
                  onClick={() => navigate("cars")}
                  className={`font-medium transition-colors ${
                    currentPage === "cars"
                      ? "text-blue-600"
                      : "text-gray-700 hover:text-blue-600"
                  }`}
                >
                  Browse Cars
                </button>

                {(currentUser.role === "client" ||
                  currentUser.role === "both") && (
                  <button
                    onClick={() => navigate("my-bookings")}
                    className={`font-medium transition-colors ${
                      currentPage === "my-bookings"
                        ? "text-blue-600"
                        : "text-gray-700 hover:text-blue-600"
                    }`}
                  >
                    My Bookings
                  </button>
                )}

                {(currentUser.role === "owner" ||
                  currentUser.role === "both") && (
                  <>
                    <button
                      onClick={() => navigate("my-cars")}
                      className={`font-medium transition-colors ${
                        currentPage === "my-cars"
                          ? "text-blue-600"
                          : "text-gray-700 hover:text-blue-600"
                      }`}
                    >
                      My Cars
                    </button>
                    <button
                      onClick={() => navigate("manage-bookings")}
                      className={`font-medium transition-colors ${
                        currentPage === "manage-bookings"
                          ? "text-blue-600"
                          : "text-gray-700 hover:text-blue-600"
                      }`}
                    >
                      Manage Bookings
                    </button>
                  </>
                )}

                <div className="flex items-center gap-4 ml-4 pl-4 border-l border-gray-200">
                  <button
                    onClick={() => navigate("profile")}
                    className="flex items-center gap-2 hover:opacity-80 transition-opacity"
                  >
                    <img
                      src={currentUser.avatar}
                      alt={currentUser.name}
                      className="w-8 h-8 rounded-full object-cover"
                    />
                    <span className="text-sm font-medium text-gray-700">
                      {currentUser.name}
                    </span>
                  </button>
                  <button
                    onClick={handleLogout}
                    className="text-gray-600 hover:text-red-600 transition-colors"
                    title="Logout"
                  >
                    <LogOut className="w-5 h-5" />
                  </button>
                </div>
              </>
            ) : (
              <>
                <button
                  onClick={() => navigate("cars")}
                  className={`font-medium transition-colors ${
                    currentPage === "cars"
                      ? "text-blue-600"
                      : "text-gray-700 hover:text-blue-600"
                  }`}
                >
                  Browse Cars
                </button>
                <button
                  onClick={() => navigate("login")}
                  className="px-4 py-2 text-blue-600 font-medium hover:bg-blue-50 rounded-lg transition-colors"
                >
                  Login
                </button>
                <button
                  onClick={() => navigate("register")}
                  className="px-4 py-2 bg-blue-600 text-white font-medium rounded-lg hover:bg-blue-700 transition-colors"
                >
                  Sign Up
                </button>
              </>
            )}
          </div>

          <button
            onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
            className="md:hidden text-gray-700"
          >
            {mobileMenuOpen ? (
              <X className="w-6 h-6" />
            ) : (
              <Menu className="w-6 h-6" />
            )}
          </button>
        </div>
      </div>

      {mobileMenuOpen && (
        <div className="md:hidden border-t border-gray-200 bg-white">
          <div className="px-4 py-3 space-y-3">
            {currentUser ? (
              <>
                <div className="flex items-center gap-3 pb-3 border-b border-gray-200">
                  <img
                    src={currentUser.avatar}
                    alt={currentUser.name}
                    className="w-10 h-10 rounded-full object-cover"
                  />
                  <div>
                    <p className="font-medium text-gray-900">
                      {currentUser.name}
                    </p>
                    <p className="text-sm text-gray-500">{currentUser.email}</p>
                  </div>
                </div>

                <button
                  onClick={() => navigate("cars")}
                  className="block w-full text-left px-3 py-2 rounded-lg text-gray-700 hover:bg-gray-50 font-medium"
                >
                  Browse Cars
                </button>

                {(currentUser.role === "client" ||
                  currentUser.role === "both") && (
                  <button
                    onClick={() => navigate("my-bookings")}
                    className="block w-full text-left px-3 py-2 rounded-lg text-gray-700 hover:bg-gray-50 font-medium"
                  >
                    My Bookings
                  </button>
                )}

                {(currentUser.role === "owner" ||
                  currentUser.role === "both") && (
                  <>
                    <button
                      onClick={() => navigate("my-cars")}
                      className="block w-full text-left px-3 py-2 rounded-lg text-gray-700 hover:bg-gray-50 font-medium"
                    >
                      My Cars
                    </button>
                    <button
                      onClick={() => navigate("manage-bookings")}
                      className="block w-full text-left px-3 py-2 rounded-lg text-gray-700 hover:bg-gray-50 font-medium"
                    >
                      Manage Bookings
                    </button>
                  </>
                )}

                <button
                  onClick={() => navigate("profile")}
                  className="block w-full text-left px-3 py-2 rounded-lg text-gray-700 hover:bg-gray-50 font-medium"
                >
                  <User className="w-4 h-4 inline mr-2" />
                  Profile
                </button>

                <button
                  onClick={handleLogout}
                  className="block w-full text-left px-3 py-2 rounded-lg text-red-600 hover:bg-red-50 font-medium"
                >
                  <LogOut className="w-4 h-4 inline mr-2" />
                  Logout
                </button>
              </>
            ) : (
              <>
                <button
                  onClick={() => navigate("cars")}
                  className="block w-full text-left px-3 py-2 rounded-lg text-gray-700 hover:bg-gray-50 font-medium"
                >
                  Browse Cars
                </button>
                <button
                  onClick={() => navigate("login")}
                  className="block w-full text-left px-3 py-2 rounded-lg text-blue-600 hover:bg-blue-50 font-medium"
                >
                  Login
                </button>
                <button
                  onClick={() => navigate("register")}
                  className="block w-full px-3 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 font-medium"
                >
                  Sign Up
                </button>
              </>
            )}
          </div>
        </div>
      )}
    </nav>
  );
};

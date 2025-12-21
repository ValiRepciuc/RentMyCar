import { Search, Shield, DollarSign, Clock } from "lucide-react";

interface HomeProps {
  onNavigate: (page: string) => void;
}

export const Home = ({ onNavigate }: HomeProps) => {
  return (
    <div className="min-h-screen bg-gradient-to-b from-blue-50 to-white">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
        <div className="flex flex-row gap-20 my-24">
          <div className="text-center mb-16">
            <h1 className="text-5xl md:text-6xl font-bold text-gray-900 mb-6">
              Find Your Perfect Ride
            </h1>
            <p className="text-xl text-gray-600 mb-8 max-w-2xl mx-auto">
              Rent cars from trusted owners in your city. Safe, affordable, and
              convenient.
            </p>
            <div className="flex flex-col sm:flex-row gap-4 justify-center">
              <button
                onClick={() => onNavigate("cars")}
                className="px-8 py-4 bg-blue-600 text-white text-lg font-semibold rounded-lg hover:bg-blue-700 transition-colors shadow-lg hover:shadow-xl"
              >
                Browse Cars
              </button>
              <button
                onClick={() => onNavigate("register")}
                className="px-8 py-4 bg-white text-blue-600 text-lg font-semibold rounded-lg hover:bg-gray-50 transition-colors shadow-lg border-2 border-blue-600"
              >
                List Your Car
              </button>
            </div>
          </div>
          <div className="flex-1">
            <img src="src/images/rental.png" alt="cars" />
          </div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8 mb-16">
          <div className="bg-white p-6 rounded-xl shadow-md hover:shadow-lg transition-shadow">
            <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center mb-4">
              <Search className="w-6 h-6 text-blue-600" />
            </div>
            <h3 className="text-xl font-bold text-gray-900 mb-2">
              Easy Search
            </h3>
            <p className="text-gray-600">
              Find the perfect car with our advanced filters by location, price,
              and features.
            </p>
          </div>

          <div className="bg-white p-6 rounded-xl shadow-md hover:shadow-lg transition-shadow">
            <div className="w-12 h-12 bg-green-100 rounded-lg flex items-center justify-center mb-4">
              <Shield className="w-6 h-6 text-green-600" />
            </div>
            <h3 className="text-xl font-bold text-gray-900 mb-2">
              Safe & Secure
            </h3>
            <p className="text-gray-600">
              All cars are verified and insured. Your safety is our top
              priority.
            </p>
          </div>

          <div className="bg-white p-6 rounded-xl shadow-md hover:shadow-lg transition-shadow">
            <div className="w-12 h-12 bg-yellow-100 rounded-lg flex items-center justify-center mb-4">
              <DollarSign className="w-6 h-6 text-yellow-600" />
            </div>
            <h3 className="text-xl font-bold text-gray-900 mb-2">
              Best Prices
            </h3>
            <p className="text-gray-600">
              Save up to 40% compared to traditional car rental companies.
            </p>
          </div>

          <div className="bg-white p-6 rounded-xl shadow-md hover:shadow-lg transition-shadow">
            <div className="w-12 h-12 bg-red-100 rounded-lg flex items-center justify-center mb-4">
              <Clock className="w-6 h-6 text-red-600" />
            </div>
            <h3 className="text-xl font-bold text-gray-900 mb-2">
              24/7 Support
            </h3>
            <p className="text-gray-600">
              Our customer support team is always ready to help you anytime.
            </p>
          </div>
        </div>

        <div className="bg-blue-600 rounded-2xl p-8 md:p-12 text-center text-white">
          <h2 className="text-3xl md:text-4xl font-bold mb-4">
            Ready to Hit the Road?
          </h2>
          <p className="text-xl mb-8 text-blue-100">
            Join thousands of satisfied customers and car owners on CarMatch
          </p>
          <button
            onClick={() => onNavigate("register")}
            className="px-8 py-4 bg-white text-blue-600 text-lg font-semibold rounded-lg hover:bg-gray-100 transition-colors shadow-lg"
          >
            Get Started Today
          </button>
        </div>
      </div>
    </div>
  );
};

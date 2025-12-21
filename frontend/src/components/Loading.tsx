import { Loader2 } from 'lucide-react';

export const Loading = () => {
  return (
    <div className="flex items-center justify-center min-h-[400px]">
      <div className="text-center">
        <Loader2 className="w-12 h-12 text-blue-600 animate-spin mx-auto mb-4" />
        <p className="text-gray-600 font-medium">Loading...</p>
      </div>
    </div>
  );
};

export const EmptyState = ({ message, icon }: { message: string; icon?: React.ReactNode }) => {
  return (
    <div className="flex items-center justify-center min-h-[400px]">
      <div className="text-center">
        {icon && <div className="mb-4 flex justify-center">{icon}</div>}
        <p className="text-gray-600 font-medium text-lg">{message}</p>
      </div>
    </div>
  );
};

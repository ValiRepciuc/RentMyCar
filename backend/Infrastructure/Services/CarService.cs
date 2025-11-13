using Domain.Entities;
using Domain.Repositories;
using Infrastructure.DTOs.Car;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public interface ICarService
{

    Task<List<CarDTO>> GetAllAsync(
        string? city = null,
        int? minPrice = null,
        int? maxPrice = null,
        string? brand = null,
        string? model = null);
    Task<CarDTO> GetByIdAsync(Guid id);
    Task<CarDTO> CreateAsync(CreateCarDTO car);
    Task<CarDTO> UpdateAsync(Guid id, UpdateCarDTO car);
    Task<CarDTO> DeleteAsync(Guid id);
}

public class CarService : BaseService, ICarService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;
    public CarService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager) : base(unitOfWork, httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<List<CarDTO>> GetAllAsync(
        string? city = null,
        int? minPrice = null,
        int? maxPrice = null,
        string? brand = null,
        string? model = null)
    {
        var cars = await _unitOfWork.Cars.QueryAsync(q =>
        {
            if (!string.IsNullOrWhiteSpace(city))
                q = q.Where(c => c.City.ToLower() == city.ToLower());

            if (minPrice.HasValue)
                q = q.Where(c => c.PricePerDay >= minPrice.Value);

            if (maxPrice.HasValue)
                q = q.Where(c => c.PricePerDay <= maxPrice.Value);

            if (!string.IsNullOrWhiteSpace(brand))
                q = q.Where(c => c.Brand.ToLower() == brand.ToLower());

            if (!string.IsNullOrWhiteSpace(model))
                q = q.Where(c => c.Model.ToLower() == model.ToLower());

            return q;
        });

        return cars.Select(c => c.ToCarDTO()).ToList();
    }

    public async Task<CarDTO> GetByIdAsync(Guid id)
    {
        var car = await _unitOfWork.Cars.GetByIdAsync(id);
        
        if (car == null)
            throw new ApplicationException($"Car with id {id} not found");
        
        var carDto = car.ToCarDTO();
        return carDto;
    }

    public async Task<CarDTO> CreateAsync(CreateCarDTO car)
    {
        var userId = GetCurrentUserId();
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            throw new Exception("User not found");

        var roles = await _userManager.GetRolesAsync(user);

        if (!roles.Contains("Owner"))
            throw new UnauthorizedAccessException("Only owners can list cars");

        ValidateCar(car);

        var addCar = car.ToCarFromCreate();
        addCar.OwnerId = userId;
        addCar.IsActive = true;

        _unitOfWork.Cars.Add(addCar);
        await _unitOfWork.SaveChangesAsync();

        return addCar.ToCarDTO();
    }


    public async Task<CarDTO> UpdateAsync(Guid id, UpdateCarDTO car)
    {
        ValidateCar(car);
        var existingCar = await _unitOfWork.Cars.GetByIdAsync(id);
        if (existingCar == null)
            throw new ApplicationException($"Car with id {id} not found");
        
        if (existingCar.OwnerId != GetCurrentUserId())
            throw new UnauthorizedAccessException("You cannot update a car you do not own.");

        car.ToCarFromUpdate(existingCar);
        await _unitOfWork.SaveChangesAsync();
        
        return existingCar.ToCarDTO();
    }

    public async Task<CarDTO> DeleteAsync(Guid id)
    {
        var existingCar = await _unitOfWork.Cars.GetByIdAsync(id);
        if (existingCar == null)
            throw new ApplicationException($"Car with id {id} not found");
        
        _unitOfWork.Cars.Delete(existingCar);
        await _unitOfWork.SaveChangesAsync();
        return existingCar.ToCarDTO();
    }
    
    private void ValidateCar(CreateCarDTO car)
    {
        if (string.IsNullOrWhiteSpace(car.Brand))
            throw new ArgumentException("Brand cannot be empty");

        if (string.IsNullOrWhiteSpace(car.Model))
            throw new ArgumentException("Model cannot be empty");

        if (car.Year < 1950 || car.Year > DateTime.UtcNow.Year + 1)
            throw new ArgumentException("Year is invalid");

        if (car.PricePerDay <= 0)
            throw new ArgumentException("Price must be greater than 0");

        if (string.IsNullOrWhiteSpace(car.City))
            throw new ArgumentException("City is required");
    }
    
    private void ValidateCar(UpdateCarDTO car)
    {
        if (string.IsNullOrWhiteSpace(car.Brand))
            throw new ArgumentException("Brand cannot be empty");
    
        if (car.PricePerDay <= 0)
            throw new ArgumentException("Price must be greater than 0");
    }


}
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.DTOs.Booking;
using Infrastructure.Enums;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public interface IBookingService
{
    Task<List<BookingDTO>> GetAllAsync();
    Task<BookingDTO> GetByIdAsync(Guid id);
    Task<BookingDTO> CreateAsync(CreateBookingDTO bookingDto);
    Task<BookingDTO> UpdateAsync(UpdateBookingDTO bookingDto, Guid id);
    Task<BookingDTO> DeleteAsync(Guid id);
    Task<BookingDTO> AcceptOrRejectAsync(AcceptOrRefuseDTO requestDto, Guid id);
    Task<List<BookingDTO>> GetUserHistoryBookingsAsync();
    Task<List<BookingDTO>> GetOwnerHistoryBookingsAsync();
}

public class BookingService : BaseService, IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    public BookingService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<BookingDTO>> GetAllAsync()
    {
        var bookings = await _unitOfWork.Bookings.GetAllFullAsync();
        var bookingsDto = bookings.Select(b => b.ToBookingDTO()).ToList();
        return bookingsDto;
    }

    public async Task<BookingDTO> GetByIdAsync(Guid id)
    {
        var booking = await _unitOfWork.Bookings.GetByIdFullAsync(id);
        if(booking == null)
            throw new Exception("Booking not found");
        
        var bookingDto = booking.ToBookingDTO();
        return bookingDto;
    }

    public async Task<BookingDTO> CreateAsync(CreateBookingDTO bookingDto)
    {
        var renterId = GetCurrentUserId();
        var renterRole = await _unitOfWork.Users.GetUserRoleAsync(renterId);

        if (renterRole != "User")
        {
            throw new Exception("Only users can make bookings");
        }
        
        var car = await _unitOfWork.Cars.GetByIdAsync(bookingDto.CarId);
        if (car == null)
            throw new Exception("Car not found.");

        if (!car.IsActive)
            throw new Exception("Car is not available.");

        var overlapping = await _unitOfWork.Bookings.QueryAsync(q =>
            q.Where(b => b.CarId == bookingDto.CarId &&
                         b.StartDate <= bookingDto.EndDate &&
                         b.EndDate >= bookingDto.StartDate &&
                         b.Status != "Rejected")
        );

        if (overlapping.Any())
            throw new Exception("This car is already booked in the selected period.");

        
        var days = bookingDto.EndDate.DayNumber - bookingDto.StartDate.DayNumber + 1;
        var total = days * car.PricePerDay;

        var booking = new Booking
        {
            CarId = bookingDto.CarId,
            StartDate = bookingDto.StartDate,
            EndDate = bookingDto.EndDate,
            TotalPrice = total,
            RenterId = renterId,
            Status = "Pending"
        };
        
        _unitOfWork.Bookings.Add(booking);
        await _unitOfWork.SaveChangesAsync();
        
        var fullBooking = await _unitOfWork.Bookings.QueryAsync(q =>
            q.Where(b => b.Id == booking.Id)
                .Include(b => b.Car)
                .Include(b => b.Renter)
        );


        return fullBooking.First().ToBookingDTO();
    }

    public async Task<BookingDTO> UpdateAsync(UpdateBookingDTO bookingDto, Guid id)
    {

        var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
        if (booking == null)
            throw new Exception("Booking not found.");

        var userId = GetCurrentUserId();
        
        if (booking.RenterId == userId && booking.Status == "Pending")
        {
            bookingDto.ToBookingFromUpdate(booking);
        }
        else
        {
            throw new Exception("You are not allowed to modify this booking.");
        }

        await _unitOfWork.SaveChangesAsync();
        return booking.ToBookingDTO();
    }
    
    public async Task<BookingDTO> DeleteAsync(Guid id)
    {
        var existingBooking = await _unitOfWork.Bookings.GetByIdAsync(id);
        
        if(existingBooking == null)
            throw new Exception("Booking not found");
        
        _unitOfWork.Bookings.Delete(existingBooking);
        await _unitOfWork.SaveChangesAsync();
        
        return existingBooking.ToBookingDTO();
    }

    public async Task<BookingDTO> AcceptOrRejectAsync(AcceptOrRefuseDTO requestDto, Guid id)
    {
        var existingBooking = await _unitOfWork.Bookings.GetByIdFullAsync(id);

        if (existingBooking == null)
            throw new Exception("Booking not found");

        if (existingBooking.Status != BookingStatusCode.Pending.ToString())
            throw new Exception("Only pending bookings can be updated");

        if (requestDto.Status == BookingStatusCode.Rejected.ToString())
        {
            existingBooking.Status = BookingStatusCode.Rejected.ToString();
            await _unitOfWork.SaveChangesAsync();
            return existingBooking.ToBookingDTO();
        }

        if (requestDto.Status == BookingStatusCode.Accepted.ToString())
        {
            existingBooking.Status = BookingStatusCode.Accepted.ToString();
            await _unitOfWork.SaveChangesAsync();
            return existingBooking.ToBookingDTO();
        }
        
        throw new Exception("Invalid booking status");
    }

    public async Task<List<BookingDTO>> GetUserHistoryBookingsAsync()
    {
        var userId = GetCurrentUserId();
        
        var bookings = await _unitOfWork.Bookings.GetAllFullAsync();
        var bookingsDto = bookings.Where(b => b.RenterId == userId).Select(b => b.ToBookingDTO()).ToList();
        
        return bookingsDto;
    }

    public async Task<List<BookingDTO>> GetOwnerHistoryBookingsAsync()
    {
        var ownerId = GetCurrentUserId();

        var bookings = await _unitOfWork.Bookings.QueryAsync(q =>
            q.Where(b => b.Car.OwnerId == ownerId)
                .OrderByDescending(b => b.StartDate)
        );

        return bookings
            .Select(b => b.ToBookingDTO())
            .ToList();
    }
}
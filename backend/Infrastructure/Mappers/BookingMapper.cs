using Domain.Entities;
using Infrastructure.DTOs.Booking;

namespace Infrastructure.Mappers;

public static class BookingMapper
{
    public static BookingDTO ToBookingDTO(this Booking booking)
    {
        return new BookingDTO
        {
            Id = booking.Id,
            CarId = booking.CarId,
            CarBrand = booking.Car.Brand,
            CarModel = booking.Car.Model,
            RenterId = booking.RenterId,
            RenterName = $"{booking.Renter.FristName} {booking.Renter.LastName}",
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status
        };
    }

    public static Booking ToBookingFromCreate(this CreateBookingDTO dto)
    {
        return new Booking
        {
            CarId = dto.CarId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };
    }

    public static void ToBookingFromUpdate(this UpdateBookingDTO dto, Booking existingBooking)
    {
        existingBooking.CarId = dto.CarId;
        existingBooking.StartDate = dto.StartDate;
        existingBooking.EndDate = dto.EndDate;
        
    }
}
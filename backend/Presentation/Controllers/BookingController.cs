using Domain.Entities;
using Infrastructure.DTOs.Booking;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/booking")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var bookings = await _bookingService.GetAllAsync();
        return Ok(bookings);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var booking = await _bookingService.GetByIdAsync(id);
        return Ok(booking);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateBookingDTO booking)
    {
        var createdBooking = await _bookingService.CreateAsync(booking);
        return Ok(createdBooking);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateBookingDTO booking)
    {
        var existingBooking = await _bookingService.UpdateAsync(booking, id);
        return Ok(existingBooking);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var booking = await _bookingService.DeleteAsync(id);
        return Ok(booking);
    }
}
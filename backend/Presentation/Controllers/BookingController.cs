using Domain.Entities;
using Infrastructure.DTOs.Booking;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
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
    [ProducesResponseType(typeof(BookingDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateBookingDTO booking)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var createdBooking = await _bookingService.CreateAsync(booking);
        return StatusCode(StatusCodes.Status201Created, createdBooking);
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

    [HttpPut("{bookingId:guid}/accept-or-reject")]
    public async Task<IActionResult> AcceptOrRejectAsync([FromBody] AcceptOrRefuseDTO requestDto,
        [FromRoute] Guid bookingId)
    {
        var result  = await _bookingService.AcceptOrRejectAsync(requestDto, bookingId);

        if (result == null)
        {
            return NoContent();
        }
        return Ok(result);
    }

    [HttpGet("/get-user-history")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetUserHistoryAsync()
    {
        var result = await _bookingService.GetUserHistoryBookingsAsync();
        return Ok(result);
    }

    [HttpGet("/get-owner-history")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> GetOwnerHistoryAsync()
    {
        var result = await _bookingService.GetOwnerHistoryBookingsAsync();
        return Ok(result);
    }
}
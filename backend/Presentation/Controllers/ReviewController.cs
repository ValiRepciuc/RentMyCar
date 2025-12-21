using Infrastructure.DTOs.Review;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/review")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet("{bookingId:guid}")]
    public async Task<IActionResult> GetReviewByBookingId([FromRoute] Guid bookingId)
    {
        var review = await _reviewService.GetReviewByBookingId(bookingId);
        return Ok(review);
    }

    [HttpPost("{bookingId:guid}")]
    public async Task<IActionResult> CreateReviewByBookingId([FromRoute] Guid bookingId, [FromBody] CreateReviewDTO reviewDto)
    {
        var createdReview = await _reviewService.CreateReviewByBookingId(bookingId, reviewDto);
        return Ok(createdReview);
    }
}
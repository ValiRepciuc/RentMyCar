using Domain.Entities;
using Domain.Repositories;
using Infrastructure.DTOs.Review;
using Infrastructure.Enums;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public interface IReviewService
{
    Task<ReviewDTO> GetReviewByBookingId(Guid bookingId);
    Task<ReviewDTO> CreateReviewByBookingId(Guid bookingId, CreateReviewDTO review);
}

public class ReviewService : BaseService, IReviewService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;
    public ReviewService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager) : base(unitOfWork, httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }
    public async Task<ReviewDTO> GetReviewByBookingId(Guid bookingId)
    {
        var review = await _unitOfWork.Reviews
            .GetByBookingIdAsync(bookingId);

        if (review == null)
            throw new Exception("Review not found for this booking");

        return review.ToReviewDTO();
    }


    public async Task<ReviewDTO> CreateReviewByBookingId(
        Guid bookingId,
        CreateReviewDTO dto)
    {
        var userId = GetCurrentUserId();
        
        var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
        if (booking == null)
            throw new Exception("Booking not found");
        
        if (booking.RenterId != userId)
            throw new Exception("You can review only your own bookings");
        
        if (booking.Status != BookingStatusCode.Accepted.ToString())
            throw new Exception("You can review only accepted bookings");
        
        var currentDate = DateOnly.FromDateTime(DateTime.Now);

        if (booking.EndDate > currentDate)
        {
            throw new Exception("Booking not finished yet");
        }
        
        var existingReview = await _unitOfWork.Reviews
            .GetByBookingIdAsync(bookingId);

        if (existingReview != null)
            throw new Exception("This booking already has a review");
        
        var review = new Review
        {
            Id = Guid.NewGuid(),
            BookingId = bookingId,
            AuthorId = userId,
            CreatedAt = DateTime.UtcNow
        };

        dto.MapFromCreate(review);

        _unitOfWork.Reviews.Add(review);
        await _unitOfWork.SaveChangesAsync();

        return review.ToReviewDTO();
    }
}
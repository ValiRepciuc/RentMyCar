using Domain.Entities;
using Infrastructure.DTOs.Review;

namespace Infrastructure.Mappers;

public static class ReviewMapper
{
    public static ReviewDTO ToReviewDTO(this Review review)
    {
        return new ReviewDTO
        {
            Id = review.Id,
            BookingId = review.BookingId,
            AuthorId = review.AuthorId,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt,
        };
    }

    public static void MapFromCreate(
        this CreateReviewDTO dto,
        Review review)
    {
        review.Rating = dto.Rating;
        review.Comment = dto.Comment;
    }

    public static void MapFromUpdate(
        this UpdateReviewDTO dto,
        Review review)
    {
        review.Rating = dto.Rating;
        review.Comment = dto.Comment;
    }
}
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs.Booking;

public class CreateBookingDTO
{
    [Required(ErrorMessage = "Car ID is required")]
    public Guid CarId { get; set; }
    
    [Required(ErrorMessage = "Start date is required")]
    public DateOnly StartDate { get; set; }
    
    [Required(ErrorMessage = "End date is required")]
    public DateOnly EndDate { get; set; }
}

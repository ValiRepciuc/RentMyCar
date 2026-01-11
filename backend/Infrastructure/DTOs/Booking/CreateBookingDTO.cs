using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs.Booking;

public class CreateBookingDTO
{
    [Required(ErrorMessage = "CarId is required")]
    public Guid CarId { get; set; }
    
    [Required(ErrorMessage = "StartDate is required")]
    public DateOnly StartDate { get; set; }
    
    [Required(ErrorMessage = "EndDate is required")]
    public DateOnly EndDate { get; set; }
}

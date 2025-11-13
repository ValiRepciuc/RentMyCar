using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppUser : IdentityUser
{
    public string FristName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string City { get; set; } = String.Empty;

    public ICollection<Car> Cars { get; set; } = new List<Car>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<SupportTicket> SupportTicketsCreated { get; set; } = new List<SupportTicket>();
    public ICollection<SupportTicket> SupportTicketsAssigned { get; set; } = new List<SupportTicket>();
}

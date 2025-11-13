namespace Domain.Entities;

public class SupportTicket : BaseEntity
{
    
    public string CreatedById { get; set; } = String.Empty;
    public AppUser CreatedBy { get; set; } = null!;
    
    public string AssignedToId { get; set; } = String.Empty;
    public AppUser AssignedTo { get; set; } = null!;
    
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string Status { get; set; } = "Open";
}
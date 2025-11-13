using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs.Account;

public class UpdateUserDTO
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string City { get; set; } = null!;
}
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs.Account;

public class RegisterDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;
    [Required]
    [MinLength(3)]
    public string UserName { get; set; } = null!;

    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string City { get; set; } = null!;

    [Required] public string Role { get; set; } = "User";

}
namespace Infrastructure.DTOs.Account;

public class NewUserDTO
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Token { get; set; } = null!;
}
using Domain.Entities;
using Infrastructure.DTOs.Account;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public interface IUserService
{
    Task<UserDTO?> UpdateUserAsync(string userId, UpdateUserDTO dto);
}

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<UserDTO?> UpdateUserAsync(string userId, UpdateUserDTO dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;
        
        UserMapper.MapToUser(user, dto);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new Exception(string.Join(";", result.Errors.Select(e => e.Description)));

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "User";

        return new UserDTO
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FristName,
            LastName = user.LastName,
            City = user.City,
            Role = role
        };
    }

}
using Domain.Entities;
using Infrastructure.DTOs.Account;

namespace Infrastructure.Mappers;

public static class UserMapper
{
    public static UserDTO ToUserDTO(this AppUser user)
    {
        return new UserDTO
        {
            Id = user.Id,
            Email = user.Email,
        };
    }
    
    public static void MapToUser(AppUser user, UpdateUserDTO dto)
    {
        user.FristName = dto.FirstName;
        user.LastName = dto.LastName;
        user.City = dto.City;

       
    }
}
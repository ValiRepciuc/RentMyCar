using Domain.Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<List<AppUser>> GetAllAsync();
    Task<AppUser?> GetByIdAsync(string id);
    Task<AppUser?> GetByUsernameAsync(string username);
    Task<string> GetUserRoleAsync(string id);
}
public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<List<AppUser>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<AppUser?> GetByIdAsync(string id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public Task<AppUser?> GetByUsernameAsync(string username)
    {  
        return _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<string> GetUserRoleAsync(string id)
    {
        var role = await _context.UserRoles
            .Where(ur => ur.UserId == id)
            .Join(_context.Roles,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => r.Name)
            .FirstOrDefaultAsync();

        return role ?? "User";
    }
}
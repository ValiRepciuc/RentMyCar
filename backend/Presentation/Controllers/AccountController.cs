using Domain.Entities;
using Infrastructure.DTOs.Account;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IUserService _userService;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, IUserService userService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var requestedRole = registerDto.Role?.Trim().ToLower();
            if (requestedRole is "admin" or "support")
                return Unauthorized("You cannot self-register as Admin or Support.");
            
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
                return BadRequest("An account with this email already exists.");
            
            var appUser = new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                FristName = registerDto.FirstName,
                LastName = registerDto.LastName,
                City = registerDto.City,
                NormalizedEmail = registerDto.Email.ToUpper(),
                NormalizedUserName = registerDto.UserName.ToUpper()
            };

            var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (!createUser.Succeeded)
                return BadRequest(createUser.Errors);
            
            var role = requestedRole == "owner" ? "Owner" : "User";
            
            var roleResult = await _userManager.AddToRoleAsync(appUser, role);
            var roles = await _userManager.GetRolesAsync(appUser);
            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            return Ok(new NewUserDTO
            {
                UserName = appUser.UserName,
                Email = appUser.Email,
                FirstName = appUser.FristName,
                LastName = appUser.LastName,
                City = appUser.City,
                Role = role,
                Token = _tokenService.CreateToken(appUser, roles)
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user == null)
            return Unauthorized("Invalid username!");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        var roles = await _userManager.GetRolesAsync(user);
        if (!result.Succeeded)
            return Unauthorized("Invalid username or password.");


        return Ok(
            new NewUserDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FristName,
                LastName = user.LastName,
                City = user.City,
                Token = _tokenService.CreateToken(user, roles)
            });
    }
    
    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("Invalid token");

            var updated = await _userService.UpdateUserAsync(userId, dto);
            if (updated == null)
                return NotFound("User not found");

            return Ok(updated);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

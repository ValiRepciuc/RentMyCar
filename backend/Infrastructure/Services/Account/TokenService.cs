using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public interface ITokenService
{
    string CreateToken(AppUser user, IList<string> roles);
    
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    public TokenService(IConfiguration config)
    {
        _config = config;
        var signingKey = _config["JWT:SigningKey"];
        if (string.IsNullOrEmpty(signingKey))
        {
            throw new InvalidOperationException("JWT:SigningKey is not configured. Please add JWT configuration to appsettings.json");
        }
        if (string.IsNullOrEmpty(_config["JWT:Issuer"]))
        {
            throw new InvalidOperationException("JWT:Issuer is not configured. Please add JWT configuration to appsettings.json");
        }
        if (string.IsNullOrEmpty(_config["JWT:Audience"]))
        {
            throw new InvalidOperationException("JWT:Audience is not configured. Please add JWT configuration to appsettings.json");
        }
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
    }
    public string CreateToken(AppUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        claims.AddRange(roles.Select(role =>
            new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds,
            Issuer = _config["JWT:Issuer"],
            Audience = _config["JWT:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
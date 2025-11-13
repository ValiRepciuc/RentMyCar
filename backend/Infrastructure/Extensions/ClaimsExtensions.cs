using System.Security.Claims;

namespace Infrastructure.Extensions;

public static class ClaimsExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
            {
                return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                       ?? throw new Exception("Error! User is not logged in.");
            }
}
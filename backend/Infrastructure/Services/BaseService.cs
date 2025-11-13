using Domain.Repositories;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public interface IBaseService
{
    string GetCurrentUserId();
}

public class BaseService : IBaseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public BaseService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User.GetUserId() 
               ?? throw new Exception("User not authenticated");
    }

    public string GetCurrentUsername()
    {
       var user = _httpContextAccessor.HttpContext?.User;
       return user?.Identity?.Name ?? "Unknown";
    }
}
using System.Security.Claims;
using Valoron.BuildingBlocks;

namespace Valoron.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) 
                               ?? _httpContextAccessor.HttpContext?.Request.Headers["x-user-id"].FirstOrDefault();

            if (Guid.TryParse(userIdString, out var userId))
            {
                return userId;
            }

            // Fallback or throw if strictly required, but for now let's return empty or handle gracefully
            // Given the middleware simulation, we should always have a header if not authenticated via other means.
            return Guid.Empty;
        }
    }
}

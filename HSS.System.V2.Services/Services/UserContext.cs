using HSS.System.V2.Services.Contracts;

using Microsoft.AspNetCore.Http;

using System.Security.Claims;

namespace HSS.System.V2.Services.Services;

/// <summary>
/// A Context that hold the basic information about the user
/// </summary>
public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Get The User Id who logged in the system
    /// </summary>
    public string ApiUserId => 
        _httpContextAccessor.HttpContext?.User.Claims
        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
}

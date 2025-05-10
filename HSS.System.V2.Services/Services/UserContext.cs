using HSS.System.V2.Services.Contracts;

using Microsoft.AspNetCore.Http;

using System.Security.Claims;

namespace HSS.System.V2.Services.Services;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string ApiUserId => 
        _httpContextAccessor.HttpContext?.User.Claims
        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
}

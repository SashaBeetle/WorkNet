using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Worknet.Shared.Helpers;
public static class UserHelper
{
    private static IHttpContextAccessor? _httpContextAccessor;

    public static void Configure(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public static string? GetCurrentUserId()
    {
        var user = _httpContextAccessor?.HttpContext?.User;
        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CommonLibrary.Helpers
{
    public static class UserSettings
    {
        public static Guid GetUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Invalid token!");
            return Guid.Parse(userId);
        }
        public static string GetUsername(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext?.User?.FindFirst(x => x.Type == ClaimTypes.Name)?.Value ?? throw new Exception("Invalid token!");
        }

        public static string GetEmail(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext?.User?.FindFirst(x => x.Type == ClaimTypes.Email)?.Value ?? throw new Exception("Invalid token!");
        }

        public static string GetToken(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString() ?? throw new Exception("Invalid token!");
        }
    }
}

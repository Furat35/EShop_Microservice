using BasketService.Api.Core.Application.Services;
using System.Security.Claims;

namespace BasketService.Api.Infrastructure.Services
{
    public class IdentityService(IHttpContextAccessor httpContextAccessor) : IIdentityService
    {
        public Guid? GetUserId()
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return string.IsNullOrEmpty(userId) ? null : Guid.Parse(userId);
        }
        public string GetUsername()
        {
            return httpContextAccessor.HttpContext?.User?.FindFirst(x => x.Type == ClaimTypes.Name)?.Value;
        }
    }
}

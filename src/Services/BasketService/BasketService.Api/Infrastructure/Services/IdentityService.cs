using BasketService.Api.Core.Application.Services;
using System.Security.Claims;

namespace BasketService.Api.Infrastructure.Services
{
    public class IdentityService(IHttpContextAccessor httpContextAccessor) : IIdentityService
    {
        public string GetUserName()
        {
            return httpContextAccessor.HttpContext?.User?.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}

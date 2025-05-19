using IdentityService.Api.Application.Models;

namespace IdentityService.Api.Application.Services.interfaces
{
    public interface IAppUserService
    {
        Task<AppUserListDto> GetUserByIdAsync(Guid id);
        Task UpdateUserAsync(AppUserUpdateDto model);
    }
}

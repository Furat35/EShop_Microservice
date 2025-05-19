using IdentityService.Api.Application.Models;

namespace IdentityService.Api.Application.Services.interfaces
{
    public interface IIdentityService
    {
        Task<bool> Register(RegisterRequestModel requestModel);
        Task<AuthResponseModel> Login(LoginRequestModel requestModel);
        Task<AuthResponseModel> RefreshTokenAsync(RefreshTokenRequestModel request);
        string HashPassword(string password, byte[] salt, int iterations = 10000, int hashByteSize = 32);
    }
}

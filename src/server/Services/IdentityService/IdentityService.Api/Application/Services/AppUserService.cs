using IdentityService.Api.Application.Models;
using IdentityService.Api.Application.Services.interfaces;
using IdentityService.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Api.Application.Services
{
    public class AppUserService(IdentityDbContext context, IIdentityService identityService) : IAppUserService
    {
        public async Task<AppUserListDto> GetUserByIdAsync(Guid id)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception();
            return new AppUserListDto
            {
                Id = id,
                Email = user.Email,
                Fullname = user.Fullname,
                Username = user.Username
            };
        }

        public async Task UpdateUserAsync(AppUserUpdateDto model)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == model.Id) ?? throw new Exception();
            if (model.Password != null)
            {
                var passwordSalt = Guid.NewGuid();
                byte[] salt = passwordSalt.ToByteArray();
                user.HashedPassword = identityService.HashPassword(model.Password, salt);
                user.PasswordSalt = passwordSalt;
            }
            user.Email = model.Email;
            user.Fullname = model.Fullname;
            user.Username = model.Username;
            await context.SaveChangesAsync();
        }
    }
}

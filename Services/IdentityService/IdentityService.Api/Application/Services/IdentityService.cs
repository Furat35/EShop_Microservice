using IdentityService.Api.Application.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Api.Application.Services
{
    public class IdentityService : IIdentityService
    {
        public Task<LoginResponseModel> Login(LoginRequestModel requestModel)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, requestModel.UserName),
                new Claim(ClaimTypes.Name, "Firat35")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("74D0A660-2080-4566-B661-6F2CE693E2B4"));
            var creds = new SigningCredentials(key ,SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(10);

            var token = new JwtSecurityToken(claims :claims, expires : expiry, signingCredentials: creds, notBefore: DateTime.Now);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new LoginResponseModel
            {
                UserToken = encodedJwt,
                UserName = requestModel.UserName
            };

            return Task.FromResult(response);
        }
    }
}

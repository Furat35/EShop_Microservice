using CommonLibrary.Exceptions;
using IdentityService.Api.Application.Models;
using IdentityService.Api.Application.Services.interfaces;
using IdentityService.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Api.Application.Services
{
    public class IdentityService(IdentityDbContext context, IConfiguration configuration) : IIdentityService
    {
        public async Task<AuthResponseModel> Login(LoginRequestModel requestModel)
        {
            var user = context.Users.FirstOrDefault(_ => _.Username == requestModel.UserName) ?? throw new NotFoundException("User not found!");
            byte[] salt = user.PasswordSalt.ToByteArray();
            var hashedPassword = HashPassword(requestModel.Password, salt);
            if (hashedPassword != user.HashedPassword)
                throw new BadRequestException("Invalid username or password!");

            var claims = new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new("fullname", user.Fullname)
            };

            var encodedJwt = GenerateJwtToken(claims);
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddDays(2);
            await context.SaveChangesAsync();

            var response = new AuthResponseModel
            {
                AccessToken = encodedJwt,
                RefreshToken = refreshToken,
                UserName = user.Username
            };

            return response;
        }

        public async Task<bool> Register(RegisterRequestModel requestModel)
        {
            var userExists = await context.Users.FirstOrDefaultAsync(u => u.Username == requestModel.UserName);
            if (userExists is not null)
                throw new BadRequestException("User already exists!");

            //önce validasyon olmalı, sonra eklenecek
            var passwordSalt = Guid.NewGuid();
            byte[] salt = passwordSalt.ToByteArray();
            var newAppUser = new AppUser
            {
                Fullname = requestModel.Fullname,
                Username = requestModel.UserName,
                Email = requestModel.Email,
                HashedPassword = HashPassword(requestModel.Password, salt),
                PasswordSalt = passwordSalt
            };
            await context.Users.AddAsync(newAppUser);
            var saveResult = await context.SaveChangesAsync();
            return saveResult != 0;
        }

        public async Task<AuthResponseModel> RefreshTokenAsync(RefreshTokenRequestModel request)
        {
            var principal = GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return null;

            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(userId) && x.RefreshToken == request.RefreshToken);

            if (user is null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return null;
            var claims = new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new("fullname", user.Fullname)
            };
            var newAccessToken = GenerateJwtToken(claims);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await context.SaveChangesAsync();

            return new AuthResponseModel
            {
                UserName = user.Username,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtTokenSettings:SecretKey"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            return securityToken is JwtSecurityToken jwtSecurityToken &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)
                ? principal
                : null;
        }

        private string GenerateJwtToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtTokenSettings:SecretKey"]));
            var expiry = DateTime.Now.AddMinutes(configuration.GetValue<int>("JwtTokenSettings:Expiry"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(claims: claims, expires: expiry, signingCredentials: creds, notBefore: DateTime.Now);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedJwt;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64]; // 512 bits
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public string HashPassword(string password, byte[] salt, int iterations = 10000, int hashByteSize = 32)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(hashByteSize);
            return Convert.ToBase64String(hash);
        }
    }
}

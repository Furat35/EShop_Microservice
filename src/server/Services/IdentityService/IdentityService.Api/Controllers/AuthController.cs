using IdentityService.Api.Application.Models;
using IdentityService.Api.Application.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IIdentityService identityService) : ControllerBase
    {
        private readonly IIdentityService identityService = identityService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel registerRequestModel)
        {
            var response = await identityService.Register(registerRequestModel);
            return response ? Ok() : BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel)
        {
            var result = await identityService.Login(loginRequestModel);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel request)
        {
            var result = await identityService.RefreshTokenAsync(request);
            return Ok(result);
        }
    }
}

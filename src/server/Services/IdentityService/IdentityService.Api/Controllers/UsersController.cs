using IdentityService.Api.Application.Models;
using IdentityService.Api.Application.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IAppUserService appUserService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppUserById(Guid id)
        {
            object x = 3;
            dynamic y = 3;
            var t = (int)y;
            var user = await appUserService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] AppUserUpdateDto model)
        {
            await appUserService.UpdateUserAsync(model);
            return Ok();
        }
    }
}

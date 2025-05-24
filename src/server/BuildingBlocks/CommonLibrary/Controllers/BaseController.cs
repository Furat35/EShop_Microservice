using CommonLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TService>(IServiceProvider servicePovider) : ControllerBase
       where TService : class
    {
        protected TService Service { get; } = servicePovider.GetRequiredService<TService>();

        [NonAction]
        public IActionResult CreateActionResult<T>(ResponseDto<T> response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return new ObjectResult(null) { StatusCode = (int)response.StatusCode };

            return new ObjectResult(response) { StatusCode = (int)response.StatusCode };
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.Api.Helpers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController(IMediator mediator) : ControllerBase
    {
        protected IMediator Mediator { get; } = mediator;

    }
}

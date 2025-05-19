using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Helpers;
using OrderService.Application.Features.Queries.GetAllOrderDetails;
using OrderService.Application.Features.Queries.GetOrderDetailById;

namespace OrderService.Api.Controllers
{
    public class OrdersController(IMediator mediator) : BaseController(mediator)
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailsById(Guid id)
        {
            var res = await Mediator.Send(new GetOrderDetailsQuery(id));
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var res = await Mediator.Send(new GetAllOrderDetailsQuery { PageIndex = pageIndex, PageSize = pageSize });
            return Ok(res);
        }
    }
}

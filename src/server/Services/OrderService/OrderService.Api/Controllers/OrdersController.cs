using CommonLibrary.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Helpers;
using OrderService.Application.Features.Queries.GetAllOrderDetails;
using OrderService.Application.Features.Queries.GetAllOrderDetailsByUserId;
using OrderService.Application.Features.Queries.GetOrderDetailById;

namespace OrderService.Api.Controllers
{
    [Authorize]
    public class OrdersController(IMediator mediator) : BaseController(mediator)
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailsById(Guid id)
        {
            var res = await Mediator.Send(new GetOrderDetailsQuery(id));
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] PaginationRequestModel request)
        {
            var res = await Mediator.Send(new GetAllOrderDetailsQuery { Page = request.Page, PageSize = request.PageSize });
            return Ok(res);
        }

        [HttpGet("byuser")]
        public async Task<IActionResult> GetOrdersByUserId([FromQuery] PaginationRequestModel request)
        {
            var res = await Mediator.Send(new GetAllOrderDetailsByUserIdQuery { Page = request.Page, PageSize = request.PageSize });
            return Ok(res);
        }
    }
}

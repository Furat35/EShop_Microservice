using CommonLibrary.Models;
using MediatR;
using OrderService.Application.Features.Queries.ViewModels;

namespace OrderService.Application.Features.Queries.GetAllOrderDetailsByUserId
{
    public class GetAllOrderDetailsByUserIdQuery : IRequest<PaginatedItemsViewModel<OrderDetailViewModel>>
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}

using CommonLibrary.Models;
using MediatR;
using OrderService.Application.Features.Queries.ViewModels;

namespace OrderService.Application.Features.Queries.GetAllOrderDetails
{
    public class GetAllOrderDetailsQuery : IRequest<PaginatedItemsViewModel<OrderDetailViewModel>>
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}

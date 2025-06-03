using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Features.Queries.ViewModels;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.Models.ViewModels;

namespace OrderService.Application.Features.Queries.GetAllOrderDetails
{
    public class GetAllOrderDetailsQueryHandler(IOrderRepository orderRepository, IMapper mapper) : IRequestHandler<GetAllOrderDetailsQuery, PaginatedItemsViewModel<OrderDetailViewModel>>
    {
        public async Task<PaginatedItemsViewModel<OrderDetailViewModel>> Handle(GetAllOrderDetailsQuery request, CancellationToken cancellationToken)
        {
            var orders = orderRepository.GetAll();
            var totalItems = await orders.CountAsync(cancellationToken);
            var itemsOnPage = await orders
                .OrderByDescending(o => o.CreateDate)
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .Include(o => o.OrderItems)
                .ToListAsync(cancellationToken);
            var mappedItems = mapper.Map<List<OrderDetailViewModel>>(itemsOnPage);
            var model = new PaginatedItemsViewModel<OrderDetailViewModel>(request.PageIndex, request.PageSize, totalItems, mappedItems);
            return model;
        }
    }
}

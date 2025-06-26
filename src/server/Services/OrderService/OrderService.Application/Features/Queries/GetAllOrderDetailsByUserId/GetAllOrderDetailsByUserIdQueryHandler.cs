using AutoMapper;
using CommonLibrary.Helpers;
using CommonLibrary.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Features.Queries.ViewModels;
using OrderService.Application.Interfaces.Repositories;

namespace OrderService.Application.Features.Queries.GetAllOrderDetailsByUserId
{
    public class GetAllOrderDetailsByUserIdQueryHandler(IOrderRepository orderRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<GetAllOrderDetailsByUserIdQuery, PaginatedItemsViewModel<OrderDetailViewModel>>
    {
        public async Task<PaginatedItemsViewModel<OrderDetailViewModel>> Handle(GetAllOrderDetailsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = orderRepository.Get(o => o.Buyer.UserId == httpContextAccessor.GetUserId(), [_ => _.Buyer]);
            var totalItems = await orders.CountAsync(cancellationToken);
            var itemsOnPage = await orders
                .OrderByDescending(o => o.CreateDate)
                .Skip(request.PageSize * request.Page)
                .Take(request.PageSize)
                .Include(o => o.OrderItems)
                .ToListAsync(cancellationToken);
            var mappedItems = mapper.Map<List<OrderDetailViewModel>>(itemsOnPage);
            var model = new PaginatedItemsViewModel<OrderDetailViewModel>(request.Page, request.PageSize, totalItems, mappedItems);
            return model;
        }
    }
}

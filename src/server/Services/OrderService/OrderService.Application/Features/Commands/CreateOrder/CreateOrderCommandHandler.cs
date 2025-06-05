using EventBus.Base.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Http;
using OrderService.Application.IntegrationEvents;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.AggregateModels.OrderAggregate;

namespace OrderService.Application.Features.Commands.CreateOrder
{
    public class CreateOrderCommandHandler(IOrderRepository orderRepository, IEventBus eventBus, IHttpContextAccessor httpContextAccessor) : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IEventBus _eventBus = eventBus;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var addr = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode);

            var dbOrder = new Order(request.UserName, request.UserId, addr, request.CardTypeId, request.CardNumber, request.CardSecurityNumber,
                request.CardHolderName, request.CardExpiration, request.Description, null);

            foreach (var orderItem in request.OrderItems)
            {
                dbOrder.AddOrderItem(orderItem.ItemId, orderItem.ItemName, orderItem.UnitPrice, orderItem.PictureUrl, orderItem.Units);
            }
            dbOrder.CalculateQuantity();
            await _orderRepository.AddAsync(dbOrder);
            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(request.UserName, request.UserId, request.Email, dbOrder.Id);

            _eventBus.Publish(orderStartedIntegrationEvent);

            return true;
        }
    }
}

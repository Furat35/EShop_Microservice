using BasketService.Api.Core.Application.Repository;
using BasketService.Api.IntegrationEvents.Events;
using EventBus.Base.Abstraction;

namespace BasketService.Api.IntegrationEvents.EventHandlers
{
    public class OrderCreatedIntegrationEventHandler(
        IBasketRepository repository,
        ILogger<OrderCreatedIntegrationEvent> logger) : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        private readonly IBasketRepository _repository = repository;
        private readonly ILogger<OrderCreatedIntegrationEvent> _logger = logger;
        public async Task Handle(OrderCreatedIntegrationEvent @event)
        {
            _logger.LogInformation("---- Handling integration event: {IntegrationEventId} at BasketService.Api - ({@IntegrationEvent})", @event.Id, @event);
            await _repository.DeleteBasketAsync(@event.UserId);
        }
    }
}

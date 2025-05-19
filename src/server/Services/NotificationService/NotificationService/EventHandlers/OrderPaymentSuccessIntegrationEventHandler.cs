using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using NotificationService.IntegrationEvents.Events;

namespace NotificationService.IntegrationEvents.EventHandlers
{
    public class OrderPaymentSuccessIntegrationEventHandler(ILogger<OrderPaymentSuccessIntegrationEventHandler> logger)
        : IIntegrationEventHandler<OrderPaymentSuccessIntegrationEvent>
    {

        public Task Handle(OrderPaymentSuccessIntegrationEvent @event)
        {
            //fake success mail sending
            logger.LogInformation($"Order with OrderId : {@event.Id} succeded");

            return Task.CompletedTask;
        }
    }
}

using EventBus.Base.Abstraction;
using EventBus.Base.Events;
using PaymentService.Api.IntegrationEvents.Events;

namespace PaymentService.Api.IntegrationEvents.EventHandlers
{
    public class OrderStartedIntegrationEventHandler(IConfiguration configuration, IEventBus eventBus, ILogger<OrderStartedIntegrationEvent> logger)
        : IIntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly IConfiguration _configuration = configuration;

        public Task Handle(OrderStartedIntegrationEvent @event)
        {
            //fake payment process

            var keyword = "PaymentSuccess";
            var paymentSuccessFlag = _configuration.GetValue<bool>(keyword);
            IntegrationEvent paymentEvent = paymentSuccessFlag
                ? new OrderPaymentSuccessIntegrationEvent(@event.OrderId, @event.Email)
                : new OrderPaymentFailedIntegrationEvent(@event.OrderId, "Payment failed!", @event.Email);

            logger.LogInformation($"OrderStartedIntegrationEventHandler in PaymentService is fired with PaymentSuccess: {paymentSuccessFlag}, orderId: {@event.OrderId}");

            eventBus.Publish(paymentEvent);
            return Task.CompletedTask;
        }
    }
}

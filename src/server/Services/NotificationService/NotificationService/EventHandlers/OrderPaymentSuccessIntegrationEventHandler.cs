using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using NotificationService.IntegrationEvents.Events;
using NotificationService.Services.Interfaces;

namespace NotificationService.IntegrationEvents.EventHandlers
{
    public class OrderPaymentSuccessIntegrationEventHandler(
        ILogger<OrderPaymentSuccessIntegrationEventHandler> logger,
        IEmailService emailService)
        : IIntegrationEventHandler<OrderPaymentSuccessIntegrationEvent>
    {

        public async Task Handle(OrderPaymentSuccessIntegrationEvent @event)
        {
            //fake success mail sending
            await emailService.SendAsync(@event.Email, "Ödeme başarılı",
                $"""
                <h1>Ödeme Tamamlandı.</h3>
                <p>{@event.OrderId}'lu siparişin ödemesi başarılı bir şekilde tamamlandı.</p>
                <p>İyi günler dileriz!</p>
                """);
            logger.LogInformation($"Order with OrderId : {@event.Id} succeded");
        }
    }
}

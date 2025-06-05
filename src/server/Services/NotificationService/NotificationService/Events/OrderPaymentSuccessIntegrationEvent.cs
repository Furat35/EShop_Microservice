using EventBus.Base.Events;

namespace NotificationService.IntegrationEvents.Events
{
    public class OrderPaymentSuccessIntegrationEvent(Guid orderId, string email) : IntegrationEvent
    {
        public Guid OrderId { get; set; } = orderId;
        public string Email { get; set; } = email;
    }
}

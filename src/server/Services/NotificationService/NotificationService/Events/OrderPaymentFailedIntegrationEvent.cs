using EventBus.Base.Events;

namespace NotificationService.IntegrationEvents.Events
{
    public class OrderPaymentFailedIntegrationEvent(Guid orderId, string errorMessage, string email) : IntegrationEvent
    {
        public Guid OrderId { get; set; } = orderId;
        public string ErrorMessage { get; set; } = errorMessage;
        public string Email { get; set; } = email;
    }
}

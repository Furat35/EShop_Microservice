using EventBus.Base.Events;

namespace OrderService.Application.IntegrationEvents
{
    public class OrderStartedIntegrationEvent(string userName, Guid userId, string email, Guid orderId) : IntegrationEvent
    {
        public string UserName { get; set; } = userName;
        public Guid UserId { get; set; } = userId;
        public string Email { get; set; } = email;
        public Guid OrderId { get; set; } = orderId;
    }
}

using MediatR;
using OrderService.Domain.AggregateModels.OrderAggregate;

namespace OrderService.Domain.Events
{
    public class OrderStartedDomainEvent(Order order, Guid buyerId,
                                   int cardTypeId, string cardNumber,
                                   string cardSecurityNumber, string cardHolderName,
                                   DateTime cardExpiration) : INotification
    {
        public Guid BuyerId { get; } = buyerId;
        public int CardTypeId { get; } = cardTypeId;
        public string CardNumber { get; } = cardNumber;
        public string CardSecurityNumber { get; } = cardSecurityNumber;
        public string CardHolderName { get; } = cardHolderName;
        public DateTime CardExpiration { get; } = cardExpiration;
        public Order Order { get; } = order;
    }
}

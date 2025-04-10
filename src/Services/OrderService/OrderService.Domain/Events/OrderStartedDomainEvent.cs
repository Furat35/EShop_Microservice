using MediatR;
using OrderService.Domain.AggregateModels.OrderAggregate;

namespace OrderService.Domain.Events
{
    public class OrderStartedDomainEvent(Order order, string userName,
                                   int cardTypeId, string cardNumber,
                                   string cardSecurityNumber, string cardHolderName,
                                   DateTime cardExpiration) : INotification
    {
        public string UserName { get; } = userName;
        public int CardTypeId { get; } = cardTypeId;
        public string CardNumber { get; } = cardNumber;
        public string CardSecurityNumber { get; } = cardSecurityNumber;
        public string CardHolderName { get; } = cardHolderName;
        public DateTime CardExpiration { get; } = cardExpiration;
        public Order Order { get; } = order;
    }
}

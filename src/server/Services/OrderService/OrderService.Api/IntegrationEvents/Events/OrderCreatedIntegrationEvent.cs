using EventBus.Base.Events;
using OrderService.Domain.Models;

namespace OrderService.Api.IntegrationEvents.Events
{
    public class OrderCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; }
        public string UserName { get; }
        public int OrderNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime CardExpiration { get; set; }
        public string CardSecurityNumber { get; set; }
        public int CardTypeId { get; set; }
        public Guid RequestId { get; set; }
        public CustomerBasket Basket { get; }
        public string Description { get; set; }

        public OrderCreatedIntegrationEvent(Guid userId, string userName, string city, string street,
            string state, string country, string zipCode, string cardNumber, string cardHolderName,
            DateTime cardExpiration, string cardSecurityNumber, int cardTypeId, Guid requestId,
            CustomerBasket basket, string description)
        {
            UserId = userId;
            UserName = userName;
            City = city;
            Street = street;
            State = state;
            Country = country;
            ZipCode = zipCode;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
            CardSecurityNumber = cardSecurityNumber;
            CardTypeId = cardTypeId;
            Basket = basket;
            RequestId = requestId;
            Description = description;
            Description = description;
        }
    }
}

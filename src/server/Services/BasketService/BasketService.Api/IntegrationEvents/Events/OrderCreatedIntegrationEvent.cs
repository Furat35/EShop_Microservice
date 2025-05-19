using BasketService.Api.Core.Domain.Models;
using EventBus.Base.Events;

namespace BasketService.Api.IntegrationEvents.Events
{
    public class OrderCreatedIntegrationEvent(string userName, string city, string street,
        string state, string country, string zipCode, string cardNumber, string cardHolderName,
        DateTime cardExpiration, string cardSecurityNumber, int cardTypeId, Guid userId,
        CustomerBasket basket, string description) : IntegrationEvent
    {
        public Guid UserId { get; set; } = userId;
        public string UserName { get; set; } = userName;
        public int OrderNumber { get; set; }
        public string City { get; set; } = city;
        public string Street { get; set; } = street;
        public string State { get; set; } = state;
        public string Country { get; set; } = country;
        public string ZipCode { get; set; } = zipCode;
        public string CardNumber { get; set; } = cardNumber;
        public string CardHolderName { get; set; } = cardHolderName;
        public DateTime CardExpiration { get; set; } = cardExpiration;
        public string CardSecurityNumber { get; set; } = cardSecurityNumber;
        public int CardTypeId { get; set; } = cardTypeId;
        public CustomerBasket Basket { get; set; } = basket;
        public string Description { get; set; } = description;

    }
}

using BasketService.Api.Core.Domain.Models;
using EventBus.Base.Events;

namespace BasketService.Api.IntegrationEvents.Events
{
    public class OrderCreatedIntegrationEvent(string city, string street,
        string state, string country, string zipCode, string cardNumber, string cardHolderName,
        DateTime cardExpiration, string cardSecurityNumber, int cardTypeId, Guid userId, string email,
        Basket basket, string description) : IntegrationEvent
    {
        public Guid UserId { get; set; } = userId;
        public string Email { get; set; } = email;
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
        public Basket Basket { get; set; } = basket;
        public string Description { get; set; } = description;

    }
}

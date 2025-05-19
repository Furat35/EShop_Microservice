using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Core.Domain.Models;
using BasketService.Api.IntegrationEvents.Events;
using CommonLibrary.Models;
using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BasketService.Api.Infrastructure.Services
{
    public class BasketService(IBasketRepository basketRepository, IEventBus eventBus,
        ILogger<BasketService> logger, IIdentityService identityService) : IBasketService
    {
        private readonly IBasketRepository _basketRepository = basketRepository;
        private readonly IEventBus _eventBus = eventBus;
        private readonly ILogger<BasketService> _logger = logger;
        private readonly IIdentityService _identityService = identityService;

        public async Task<ResponseDto<bool>> DeleteBasketAsync(Guid id)
        {
            if (!CheckIfUsersBasket(id))
                return ResponseDto<bool>.Fail("Not authorized!", HttpStatusCode.Forbidden);
            var response = await _basketRepository.DeleteBasketAsync(id);
            return ResponseDto<bool>.GenerateResponse(response)
                .Success(response, HttpStatusCode.OK)
                .Fail("Error occured while deleting!", HttpStatusCode.BadRequest);
        }

        public async Task<ResponseDto<CustomerBasket?>> GetBasketAsync(Guid id)
        {
            if (!CheckIfUsersBasket(id))
                return ResponseDto<CustomerBasket?>.Fail("Not authorized!", HttpStatusCode.Forbidden);
            var basket = await _basketRepository.GetBasketAsync(id);
            return ResponseDto<CustomerBasket?>.Success(basket, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<CustomerBasket?>> UpdateBasketAsync(CustomerBasket basket)
        {
            if (!CheckIfUsersBasket(basket.UserId))
                return ResponseDto<CustomerBasket?>.Fail("Not authorized!", HttpStatusCode.Forbidden);
            var response = await _basketRepository.UpdateBasketAsync(basket);
            return ResponseDto<CustomerBasket?>.GenerateResponse(response != null)
               .Success(response, HttpStatusCode.OK)
               .Fail("Error occured while updating Basket!", HttpStatusCode.NotFound);
        }

        public async Task<ResponseDto<bool>> CheckoutAsync([FromBody] BasketCheckout basketCheckout)
        {
            var userId = basketCheckout.UserId;
            if (!CheckIfUsersBasket(userId))
                return ResponseDto<bool>.Fail("Not authorized!", HttpStatusCode.Forbidden);
            var username = _identityService.GetUsername().ToString();
            var basket = await _basketRepository.GetBasketAsync(userId);

            if (basket == null)
                return ResponseDto<bool>.Fail("Empty basket", HttpStatusCode.BadRequest);

            var eventMessage = new OrderCreatedIntegrationEvent(username, basketCheckout.City, basketCheckout.Street,
                basketCheckout.State, basketCheckout.Country, basketCheckout.ZipCode, basketCheckout.CardNumber, basketCheckout.CardHolderName,
                basketCheckout.CardExpiration, basketCheckout.CardSecurityNumber, basketCheckout.CardTypeId, basketCheckout.UserId, basket, basketCheckout.Description);

            try
            {
                _eventBus.Publish(eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {BasketService.App}", eventMessage.Id);
                throw;
            }

            return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
        }

        private bool CheckIfUsersBasket(Guid id)
        {
            var userId = _identityService.GetUserId();
            return userId.Value == id;
        }
    }
}

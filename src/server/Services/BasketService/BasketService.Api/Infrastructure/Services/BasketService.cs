using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Core.Domain.Models;
using BasketService.Api.IntegrationEvents.Events;
using CommonLibrary.Models;
using Discount.gRPC;
using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BasketService.Api.Infrastructure.Services
{
    public class BasketService(IBasketRepository basketRepository, IEventBus eventBus,
        ILogger<BasketService> logger, IIdentityService identityService,
        Discount.gRPC.DiscountService.DiscountServiceClient discountService) : IBasketService
    {
        private readonly IBasketRepository _basketRepository = basketRepository;
        private readonly IEventBus _eventBus = eventBus;
        private readonly ILogger<BasketService> _logger = logger;
        private readonly Guid _userId = identityService.GetUserId().Value;

        public async Task<ResponseDto<bool>> DeleteBasketAsync()
        {
            var response = await _basketRepository.DeleteBasketAsync(_userId);
            return ResponseDto<bool>.GenerateResponse(response)
                .Success(response, HttpStatusCode.OK)
                .Fail("Error occured while deleting!", HttpStatusCode.BadRequest);
        }

        public async Task<ResponseDto<Basket?>> GetBasketAsync()
        {
            var basket = await _basketRepository.GetBasketAsync(_userId);
            if(basket?.Items.Count > 0)
            {
                var request = new ItemDiscountsRequestModel();
                request.ItemIds.AddRange(basket.Items.Select(_ => _.ItemId));
                var discounts = await discountService.GetDiscountsByItemIdsAsync(request);
                if (discounts is not null)
                {
                    BasketItem basketItem = null;
                    foreach (var discount in discounts.Discounts)
                    {
                        basketItem = basket.Items.First(_ => _.ItemId == discount.ItemId);
                        basketItem.DiscountAmount = (decimal)discount.Amount > (decimal)discount.Percentage * basketItem.UnitPrice
                            ? (decimal)discount.Amount
                            : (decimal)discount.Percentage * basketItem.UnitPrice / 100;
                    }
                }
            }
            
            return ResponseDto<Basket?>.Success(basket, HttpStatusCode.OK);
        }

        public async Task RefreshBasket()
        {
            var basket = await _basketRepository.GetBasketAsync(_userId);
           
        }

        public async Task<ResponseDto<bool>> UpdateBasketAsync(Basket basket)
        {
            basket.UserId = _userId;
            var response = await _basketRepository.UpdateBasketAsync(basket);
            return ResponseDto<bool>.GenerateResponse(response)
               .Success(response, HttpStatusCode.OK)
               .Fail("Error occured while updating Basket!", HttpStatusCode.NotFound);
        }

        public async Task<ResponseDto<bool>> CheckoutAsync([FromBody] BasketCheckout basketCheckout)
        {
            basketCheckout.UserId = _userId;
            //var username = _identityService.GetUsername().ToString();
            var basket = await _basketRepository.GetBasketAsync(_userId);

            if (basket == null)
                return ResponseDto<bool>.Fail("Empty basket", HttpStatusCode.BadRequest);

            var eventMessage = new OrderCreatedIntegrationEvent(basketCheckout.City, basketCheckout.Street,
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
    }
}

using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Core.Domain.Models;
using BasketService.Api.IntegrationEvents.Events;
using CommonLibrary.Helpers;
using CommonLibrary.Models;
using Discount.gRPC;
using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BasketService.Api.Infrastructure.Services
{
    // Öyle bir ürünün olup olmadığı kontrol edildikten sonra sepete ekleme işlemi de yapılabilir
    public class BasketService(IBasketRepository basketRepository, IEventBus eventBus,
        ILogger<BasketService> logger, IHttpContextAccessor httpContext,
        DiscountService.DiscountServiceClient discountService) : IBasketService
    {
        private readonly IBasketRepository _basketRepository = basketRepository;
        private readonly IEventBus _eventBus = eventBus;
        private readonly ILogger<BasketService> _logger = logger;
        private readonly Guid _userId = httpContext.GetUserId();

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
            await ApplyDiscount(basket);
            return ResponseDto<Basket?>.Success(basket, HttpStatusCode.OK);
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
            var basket = await GetBasketAsync();

            if (basket == null)
                return ResponseDto<bool>.Fail("Empty basket", HttpStatusCode.BadRequest);

            var eventMessage = new OrderCreatedIntegrationEvent(basketCheckout.City, basketCheckout.Street,
                basketCheckout.State, basketCheckout.Country, basketCheckout.ZipCode, basketCheckout.CardNumber, basketCheckout.CardHolderName,
                basketCheckout.CardExpiration, basketCheckout.CardSecurityNumber, basketCheckout.CardTypeId, basketCheckout.UserId,
                httpContext.GetEmail(), basket.Data, basketCheckout.Description);

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

        private async Task ApplyDiscount(Basket basket)
        {
            if (basket?.Items.Count > 0)
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
        }
    }
}

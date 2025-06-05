using BasketService.Api.Core.Application.Services;
using BasketService.Api.Core.Domain.Models;
using CommonLibrary.Controllers;
using CommonLibrary.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.Api.Controllers
{
    [Authorize]
    public class BasketController(
        IServiceProvider services,
        IHttpContextAccessor httpClient) : BaseController<IBasketService>(services)
    {
        private readonly Guid _userId = httpClient.GetUserId();

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var basket = await Service.GetBasketAsync();
            basket.Data ??= new Basket(_userId);
            return CreateActionResult(basket);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateBasketAsync([FromBody] Basket basket)
        {
            return CreateActionResult(await Service.UpdateBasketAsync(basket));
        }

        [Route("additem")]
        [HttpPost]
        public async Task<IActionResult> AddItemToBasket([FromBody] BasketItem basketItem)
        {
            var basket = await Service.GetBasketAsync();
            basket.Data ??= new Basket(_userId);
            var basketItemExists = basket.Data.Items.FirstOrDefault(_ => _.ItemId == basketItem.ItemId);
            if (basketItemExists is null)
                basket.Data.Items.Add(basketItem);
            else
                basketItemExists.Quantity += basketItem.Quantity;
            var response = await Service.UpdateBasketAsync(basket.Data);

            return CreateActionResult(response);
        }

        [Route("checkout")]
        [HttpPost]
        public async Task<IActionResult> CheckoutAsync([FromBody] BasketCheckout basketCheckout)
        {
            return CreateActionResult(await Service.CheckoutAsync(basketCheckout));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasketByIdAsync()
        {
            return CreateActionResult(await Service.DeleteBasketAsync());
        }
    }
}

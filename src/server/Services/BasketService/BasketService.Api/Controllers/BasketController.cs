using BasketService.Api.Core.Application.Services;
using BasketService.Api.Core.Domain.Models;
using BasketService.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.Api.Controllers
{
    [Authorize]
    public class BasketController(
        IServiceProvider services,
        IIdentityService identityService) : BaseController<IBasketService>(services)
    {
        private readonly IIdentityService _identityService = identityService;

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Basket Service is Up and Running");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBasketByIdAsync(Guid id)
        {
            var basket = await Service.GetBasketAsync(id);
            basket.Data ??= new CustomerBasket(id);
            return CreateActionResult(basket);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateBasketAsync([FromBody] CustomerBasket basket)
        {
            return CreateActionResult(await Service.UpdateBasketAsync(basket));
        }

        [Route("additem")]
        [HttpPost]
        public async Task<IActionResult> AddItemToBasket([FromBody] BasketItem basketItem)
        {
            var userId = _identityService.GetUserId();
            var basket = await Service.GetBasketAsync(userId.Value);
            basket.Data ??= new CustomerBasket(userId.Value);
            basket.Data.Items.Add(basketItem);
            basket = await Service.UpdateBasketAsync(basket.Data);

            return CreateActionResult(basket);
        }

        [Route("checkout")]
        [HttpPost]
        public async Task<IActionResult> CheckoutAsync([FromBody] BasketCheckout basketCheckout)
        {
            return CreateActionResult(await Service.CheckoutAsync(basketCheckout));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketByIdAsync(Guid id)
        {
            return CreateActionResult(await Service.DeleteBasketAsync(id));
        }
    }
}

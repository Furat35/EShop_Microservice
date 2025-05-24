using BasketService.Api.Core.Domain.Models;
using CommonLibrary.Models;

namespace BasketService.Api.Core.Application.Services
{
    public interface IBasketService
    {
        Task<ResponseDto<Basket?>> GetBasketAsync();
        Task<ResponseDto<bool>> UpdateBasketAsync(Basket basket);
        Task<ResponseDto<bool>> DeleteBasketAsync();
        Task<ResponseDto<bool>> CheckoutAsync(BasketCheckout basketCheckout);
    }
}

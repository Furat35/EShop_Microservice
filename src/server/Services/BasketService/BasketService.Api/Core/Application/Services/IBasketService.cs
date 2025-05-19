using BasketService.Api.Core.Domain.Models;
using CommonLibrary.Models;

namespace BasketService.Api.Core.Application.Services
{
    public interface IBasketService
    {
        Task<ResponseDto<CustomerBasket?>> GetBasketAsync(Guid id);
        Task<ResponseDto<CustomerBasket?>> UpdateBasketAsync(CustomerBasket basket);
        Task<ResponseDto<bool>> DeleteBasketAsync(Guid id);
        Task<ResponseDto<bool>> CheckoutAsync(BasketCheckout basketCheckout);
    }
}

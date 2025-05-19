using BasketService.Api.Core.Domain.Models;

namespace BasketService.Api.Core.Application.Repository
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(Guid id);
        IEnumerable<string> GetUsers();
        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(Guid id);
    }
}

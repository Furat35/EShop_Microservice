using BasketService.Api.Core.Domain.Models;

namespace BasketService.Api.Core.Application.Repository
{
    public interface IBasketRepository
    {
        Task<Basket?> GetBasketAsync(Guid id);
        IEnumerable<string> GetUsers();
        Task<bool> UpdateBasketAsync(Basket basket);
        Task<bool> DeleteBasketAsync(Guid id);
    }
}

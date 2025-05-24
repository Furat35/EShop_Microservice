using CommonLibrary.Repositories.Interfaces;

namespace Discount.Api.Repositories.Interfaces
{
    public interface IDiscountRepository : IGenericRepository<Models.Discount, int>
    {
    }
}

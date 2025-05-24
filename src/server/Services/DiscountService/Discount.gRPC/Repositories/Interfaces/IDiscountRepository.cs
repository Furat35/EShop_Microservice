using CommonLibrary.Repositories.Interfaces;

namespace Discount.gRPC.Repositories.Interfaces
{
    public interface IDiscountRepository : IGenericRepository<Models.Discount, int>
    {
    }
}

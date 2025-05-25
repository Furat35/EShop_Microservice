using CommonLibrary.Repositories.Interfaces;

namespace Discount.gRPC.Repositories.Interfaces
{
    public interface ICatalogItemRepository : IGenericRepository<Models.CatalogItem, int>
    {
    }
}

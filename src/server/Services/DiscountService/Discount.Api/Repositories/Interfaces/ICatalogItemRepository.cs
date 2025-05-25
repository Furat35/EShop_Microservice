using CommonLibrary.Repositories.Interfaces;

namespace Discount.Api.Repositories.Interfaces
{
    public interface ICatalogItemRepository : IGenericRepository<Models.CatalogItem, int>
    {
    }
}

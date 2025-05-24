using CommonLibrary.Repositories.Interfaces;
using System;

namespace Discount.gRPC.Repositories.Interfaces
{
    public interface ICatalogItemRepository : IGenericRepository<Models.CatalogItem, int>
    {
    }
}

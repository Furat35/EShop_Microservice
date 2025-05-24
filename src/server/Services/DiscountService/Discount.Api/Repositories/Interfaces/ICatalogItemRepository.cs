using CommonLibrary.Repositories.Interfaces;
using System;

namespace Discount.Api.Repositories.Interfaces
{
    public interface ICatalogItemRepository : IGenericRepository<Models.CatalogItem, int>
    {
    }
}

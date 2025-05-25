using CommonLibrary.Repositories;
using Discount.Api.Repositories.Context;
using Discount.Api.Repositories.Interfaces;

namespace Discount.Api.Repositories
{
    public class CatalogItemRepository(DiscountDbContext context) : GenericRepository<DiscountDbContext, Models.CatalogItem, int>(context), ICatalogItemRepository
    {
    }
}

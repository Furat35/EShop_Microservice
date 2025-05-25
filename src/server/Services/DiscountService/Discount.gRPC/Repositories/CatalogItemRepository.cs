using CommonLibrary.Repositories;
using Discount.gRPC.Repositories.Context;
using Discount.gRPC.Repositories.Interfaces;

namespace Discount.gRPC.Repositories
{
    public class CatalogItemRepository(DiscountDbContext context) : GenericRepository<DiscountDbContext, Models.CatalogItem, int>(context), ICatalogItemRepository
    {
    }
}

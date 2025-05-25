using CommonLibrary.Repositories;
using Discount.Api.Repositories.Context;
using Discount.Api.Repositories.Interfaces;

namespace Discount.Api.Repositories
{
    public class DiscountRepository(DiscountDbContext context) : GenericRepository<DiscountDbContext, Models.Discount, int>(context), IDiscountRepository
    {
    }
}

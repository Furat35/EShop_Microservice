using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Infrastructure.Context;

namespace OrderService.Infrastructure.Repositories
{
    public class BuyerRepository(OrderDbContext dbContext) : GenericRepository<Buyer>(dbContext), IBuyerRepository
    {
    }
}

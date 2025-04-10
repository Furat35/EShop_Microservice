using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Infrastructure.Context;
using System.Linq.Expressions;

namespace OrderService.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext dbContext) : GenericRepository<Order>(dbContext), IOrderRepository
    {
        public override async Task<Order> GetByIdAsync(Guid id, params Expression<Func<Order, object>>[] includes)
        {
            return await base.GetByIdAsync(id, includes) ?? dbContext.Orders.Local.FirstOrDefault(i => i.Id == id);
        }
    }
}

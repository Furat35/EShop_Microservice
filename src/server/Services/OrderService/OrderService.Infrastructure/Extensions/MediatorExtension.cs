using MediatR;
using OrderService.Domain.SeedWork;
using OrderService.Infrastructure.Context;

namespace OrderService.Infrastructure.Extensions
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, OrderDbContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                                    .Entries<BaseEntity>()
                                    .Where(x => x.Entity.DomainEvents?.Any() == true)
                                    .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);

            if (ctx.ChangeTracker
                .Entries<BaseEntity>()
                .Any(x => x.Entity.DomainEvents?.Any() == true))
            {
                await DispatchDomainEventsAsync(mediator, ctx);
            }
        }
    }
}

using EventBus.Base.Abstraction;
using MediatR;
using OrderService.Api.IntegrationEvents.Events;
using OrderService.Application.Features.Commands.CreateOrder;

namespace OrderService.Api.IntegrationEvents.EventHandlers
{
    public class OrderCreatedIntegrationEventHandler(IMediator mediator, ILogger<OrderCreatedIntegrationEventHandler> logger)
        : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<OrderCreatedIntegrationEventHandler> _logger = logger;

        public async Task Handle(OrderCreatedIntegrationEvent @event)
        {
            try
            {
                _logger.LogInformation("Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})",
                @event.Id,
                typeof(Program).Namespace,
                @event);

                var createOrderCommand = new CreateOrderCommand(@event.Basket.Items,
                                @event.UserId, @event.UserName,
                                @event.City, @event.Street,
                                @event.State, @event.Country, @event.ZipCode,
                                @event.CardNumber, @event.CardHolderName, @event.CardExpiration,
                                @event.CardSecurityNumber, @event.CardTypeId, @event.Description);

                await _mediator.Send(createOrderCommand);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.ToString());
            }
        }
    }
}

﻿using MediatR;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Domain.Events;

namespace OrderService.Application.DomainEventHandlers
{
    public class OrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
    {
        private readonly IBuyerRepository buyerRepository;

        public OrderStartedDomainEventHandler(IBuyerRepository buyerRepository)
        {
            this.buyerRepository = buyerRepository;
        }

        public async Task Handle(OrderStartedDomainEvent orderStartedEvent, CancellationToken cancellationToken)
        {
            var cardTypeId = (orderStartedEvent.CardTypeId != 0) ? orderStartedEvent.CardTypeId : 1;

            var buyer = await buyerRepository.GetSingleAsync(i => i.UserId == orderStartedEvent.BuyerId, i => i.PaymentMethods);

            bool buyerOriginallyExisted = buyer != null;

            if (!buyerOriginallyExisted)
            {
                buyer = new Buyer(orderStartedEvent.BuyerId);
            }

            buyer.VerifyOrAddPaymentMethod(cardTypeId,
                                           $"Payment Method on {DateTime.UtcNow}",
                                           orderStartedEvent.CardNumber,
                                           orderStartedEvent.CardSecurityNumber,
                                           orderStartedEvent.CardHolderName,
                                           orderStartedEvent.CardExpiration,
                                           orderStartedEvent.Order.Id);

            var buyerUpdated = buyerOriginallyExisted ?
                buyerRepository.Update(buyer) :
                await buyerRepository.AddAsync(buyer);

            // order status changed event may be fired here
        }
    }
}

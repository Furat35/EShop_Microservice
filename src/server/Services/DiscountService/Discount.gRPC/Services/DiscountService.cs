using Discount.gRPC;
using Discount.gRPC.Models;
using Discount.gRPC.Repositories.Interfaces;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Services
{
    public class DiscountService(IDiscountRepository discountRepository) : gRPC.DiscountService.DiscountServiceBase
    {
        public override async Task<DiscountReply?> GetDiscountByItemId(ItemDiscountRequestModel request, ServerCallContext context)
        {
            var discount = await discountRepository.GetAll().Include(d => d.Items).FirstOrDefaultAsync(d => d.Items.Any(_ => _.Id == request.ItemId));
            if (discount is null) throw new RpcException(new Status(StatusCode.NotFound, $"Discount not found for ItemId: {request.ItemId}"));

            var discountReply = new DiscountReply 
            { 
                ItemId = request.ItemId,    
                DiscountId = discount.Id,
                Amount = discount.Amount,
                Percentage = discount.Percentage,
            };

            return discountReply;
        }

        public override async Task<DiscountListReply> GetDiscountsByItemIds(ItemDiscountsRequestModel request, ServerCallContext context)
        {
            Models.Discount? discount = null;
            var response = new DiscountListReply();
            foreach (var itemId in request.ItemIds)
            {
                discount = await discountRepository.GetAll().Include(d => d.Items).FirstOrDefaultAsync(d => d.Items.Any(_ => _.Id == itemId));
                response.Discounts.Add(new DiscountReply()
                {
                    ItemId = itemId,
                    DiscountId = discount.Id,
                    Amount = discount.Amount,
                    Percentage = discount.Percentage,
                });
            }

            return response;
        }
    }
}

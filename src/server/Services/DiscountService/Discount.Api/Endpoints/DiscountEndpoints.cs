using Discount.Api.Models;
using Discount.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Discount.Api.Endpoints
{
    public static class DiscountEndpoints
    {
        public static void RegisterDiscountEndpoints(this WebApplication app)
        {
            app.MapGet("/discounts", ([FromServices] IDiscountRepository discountRepository) =>
            {
                var discounts = discountRepository.GetAll().ToList();
                return discounts;
            });

            app.MapGet("/discounts/{id}", (int id, [FromServices] IDiscountRepository discountRepository) =>
            {
                return discountRepository.GetByIdAsync(id);
            });

            app.MapPost("/discounts", async ([FromBody] Models.Discount discount, [FromServices] IDiscountRepository discountRepository) =>
            {
                await discountRepository.AddAsync(discount);
                return await discountRepository.SaveChangesAsync();
            }).RequireAuthorization();

            app.MapPost("/discounts/addToItem/{discountId:int}/{itemId:int}", async (int discountId, int itemId,
                [FromServices] IDiscountRepository discountRepository, [FromServices] ICatalogItemRepository productRepository) =>
            {
                var discount = await discountRepository.GetByIdAsync(discountId, false);
                if (discount is null) throw new Exception("Discount doesn't exist");
                await productRepository.AddAsync(new CatalogItem { Id = itemId, DiscountId = discountId });
                return await productRepository.SaveChangesAsync();
            }).RequireAuthorization();

            app.MapPut("/discounts/addToItem/{discountId:int}/{itemId:int}", async (int discountId, int itemId,
                [FromServices] IDiscountRepository discountRepository, [FromServices] ICatalogItemRepository productRepository) =>
            {
                var discount = await discountRepository.GetByIdAsync(discountId, false);
                if (discount is null) throw new Exception("Discount doesn't exist");
                productRepository.Update(new CatalogItem { Id = itemId, DiscountId = discountId });
                return await productRepository.SaveChangesAsync();
            }).RequireAuthorization();

            app.MapPut("/discounts", async ([FromBody] Models.Discount discount, [FromServices] IDiscountRepository discountRepository) =>
            {
                var discounts = discountRepository.Update(discount);
                await discountRepository.SaveChangesAsync();
                return discounts;
            }).RequireAuthorization();
        }
    }
}

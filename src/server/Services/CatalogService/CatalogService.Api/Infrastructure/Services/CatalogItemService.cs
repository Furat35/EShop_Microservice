using CatalogService.Api.Core.Application.Services;
using CatalogService.Api.Core.Domain;
using CatalogService.Api.Infrastructure.Context;
using CommonLibrary.Models;
using CommonLibrary.Repositories;
using Discount.gRPC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;

namespace CatalogService.Api.Infrastructure.Services
{
    public class CatalogItemService(
        CatalogContext dbContext,
        IOptionsSnapshot<CatalogSettings> settings,
        DiscountService.DiscountServiceClient discountService)
        : GenericRepository<CatalogContext, CatalogItem, int>(dbContext), ICatalogItemService
    {
        private readonly CatalogSettings _settings = settings.Value;
        public async Task<ResponseDto<PaginatedItemsViewModel<CatalogItem>>> GetItemsAsync(int page = 0, int pageSize = 10, string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = await GetItemsByIds(ids);
                await ApplyDiscount(items);
                var paginatedItems = new PaginatedItemsViewModel<CatalogItem>(page, pageSize, items.Count, items);
                return ResponseDto<PaginatedItemsViewModel<CatalogItem>>
                    .GenerateResponse(items.Count > 0)
                    .Success(paginatedItems, HttpStatusCode.OK)
                    .Fail("ids value invalid. Must be comma-separated list of numbers", HttpStatusCode.BadRequest);
            }

            var totalItems = await GetAll().LongCountAsync();
            var itemsOnPage = await GetAll()
                .OrderBy(c => c.Name)
                .Skip(pageSize * page)
                .Take(pageSize)
                .Include(c => c.CatalogType)
                .Include(c => c.CatalogBrand)
                .ToListAsync();
            await ApplyDiscount(itemsOnPage);
            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            var model = new PaginatedItemsViewModel<CatalogItem>(page, pageSize, totalItems, itemsOnPage);
            return ResponseDto<PaginatedItemsViewModel<CatalogItem>>.Success(model, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<CatalogItem>> GetItemByIdAsync(int id)
        {
            if (id <= 0)
                return ResponseDto<CatalogItem>.Fail("Enter a valid id", HttpStatusCode.BadRequest);

            var item = await GetByIdAsync(id);
            var baseUri = _settings.PicBaseUrl;

            if (item is null)
                return ResponseDto<CatalogItem>.Fail("Catalog could not be found!", HttpStatusCode.NotFound);

            item.PictureUri = baseUri + item.PictureFileName;
            await ApplyDiscount(item);
            return ResponseDto<CatalogItem>.Success(item, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<PaginatedItemsViewModel<CatalogItem>>> ItemsWithNameAsync(string name, int pageSize = 10, int page = 0)
        {
            var totalItems = await GetAll()
                .Where(c => c.Name.Contains(name))
                .LongCountAsync();

            var itemsOnPage = await GetAll()
                .Where(c => c.Name.Contains(name))
                .Skip(pageSize * page)
                .Take(pageSize)
                .Include(c => c.CatalogType)
                .Include(c => c.CatalogBrand)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);
            await ApplyDiscount(itemsOnPage);
            var response = new PaginatedItemsViewModel<CatalogItem>(page, pageSize, totalItems, itemsOnPage);

            return ResponseDto<PaginatedItemsViewModel<CatalogItem>>.Success(response, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<PaginatedItemsViewModel<CatalogItem>>> ItemsByTypeIdAndBrandIdAsync(int? catalogTypeId, int? catalogBrandId, int pageSize, int page)
        {
            var root = GetAll();
            if (catalogTypeId.HasValue)
                root = root.Where(ci => ci.CatalogTypeId == catalogTypeId);

            if (catalogBrandId.HasValue)
                root = root.Where(ci => ci.CatalogBrandId == catalogBrandId);

            var totalItems = await root
                .LongCountAsync();

            var itemsOnPage = await root
                .Skip(pageSize * page)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);
            await ApplyDiscount(itemsOnPage);

            var response = new PaginatedItemsViewModel<CatalogItem>(page, pageSize, totalItems, itemsOnPage);
            return ResponseDto<PaginatedItemsViewModel<CatalogItem>>.Success(response, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<bool>> UpdateItemAsync([FromBody] CatalogItem productToUpdate)
        {
            var item = await GetByIdAsync(productToUpdate.Id, false);
            if (item is null)
                return ResponseDto<bool>.Fail($"Item : {productToUpdate.Name} not found.", HttpStatusCode.NotFound);

            var oldPrice = item.Price;
            var raiseProductPriceChangedEvent = oldPrice != productToUpdate.Price;

            // Update current product
            Update(productToUpdate);
            //if (raiseProductPriceChangedEvent) // Save product's data and publish integration event through the Event Bus if price has changed
            //{
            //    //Create Integration Event to be published through the Event Bus
            //    var priceChangedEvent = new ProductPriceChangedIntegrationEvent(catalogItem.Id, productToUpdate.Price, oldPrice);

            //    // Achieving atomicity between original Catalog database operation and the IntegrationEventLog thanks to a local transaction
            //    await _catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(priceChangedEvent);

            //    // Publish through the Event Bus and mark the saved event as published
            //    await _catalogIntegrationEventService.PublishThroughEventBusAsync(priceChangedEvent);
            //}
            //else // Just save the updated product because the Product's Price hasn't changed.
            //{

            //}

            return ResponseDto<bool>.GenerateResponse(await SaveChangesAsync() > 0)
                    .Success(true, HttpStatusCode.OK)
                    .Fail("Error occured while deleting catalog!", HttpStatusCode.InternalServerError);
        }

        public async Task<ResponseDto<bool>> CreateItemAsync(CatalogItem product)
        {
            var item = new CatalogItem
            {
                CatalogBrandId = product.CatalogBrandId,
                CatalogTypeId = product.CatalogTypeId,
                Description = product.Description,
                Name = product.Name,
                PictureFileName = product.PictureFileName,
                AvailableStock = product.AvailableStock,
                Price = product.Price
            };

            await AddAsync(item);
            return ResponseDto<bool>.GenerateResponse(await SaveChangesAsync() > 0)
                    .Success(true, HttpStatusCode.OK)
                    .Fail("Error occured while deleting catalog type!", HttpStatusCode.InternalServerError);
        }

        public async Task<ResponseDto<bool>> DeleteItemAsync(int id)
        {
            await DeleteByIdAsync(id);
            return ResponseDto<bool>.GenerateResponse(await SaveChangesAsync() > 0)
                    .Success(true, HttpStatusCode.OK)
                    .Fail("Error occured while deleting catalog type!", HttpStatusCode.InternalServerError);
        }

        private async Task<List<CatalogItem>> GetItemsByIds(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));
            if (!numIds.All(nid => nid.Ok))
                return [];

            var idsToSelect = numIds
                .Select(id => id.Value);
            var items = await GetAll().Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();
            items = ChangeUriPlaceholder(items);

            return items;
        }

        private List<CatalogItem> ChangeUriPlaceholder(List<CatalogItem> items)
        {
            var baseUri = _settings.PicBaseUrl;

            foreach (var item in items)
            {
                if (item != null)
                    item.PictureUri = baseUri + item.PictureFileName;
            }

            return items;
        }

        private async Task ApplyDiscount(List<CatalogItem> catalogItems)
        {
            var request = new ItemDiscountsRequestModel();
            request.ItemIds.AddRange(catalogItems.Select(_ => _.Id));
            var discounts = await discountService.GetDiscountsByItemIdsAsync(request);
            if (discounts is not null)
            {
                CatalogItem catalogItem = null;
                foreach (var discount in discounts.Discounts)
                {
                    catalogItem = catalogItems.First(_ => _.Id == discount.ItemId);
                    catalogItem.DiscountAmount = (decimal)discount.Amount > (decimal)discount.Percentage * catalogItem.Price / 100
                        ? (decimal)discount.Amount
                        : (decimal)discount.Percentage * catalogItem.Price / 100;
                }
            }
        }

        private async Task ApplyDiscount(CatalogItem catalogItem)
        {
            if (catalogItem is not null)
                await ApplyDiscount([catalogItem]);
        }
    }
}

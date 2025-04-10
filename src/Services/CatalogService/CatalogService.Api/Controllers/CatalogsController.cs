using CatalogService.Api.Core.Application.ViewModels;
using CatalogService.Api.Core.Domain;
using CatalogService.Api.Infrastructure;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogsController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;
        private readonly CatalogSettings _settings;

        public CatalogsController(CatalogContext context, IOptionsSnapshot<CatalogSettings> settings)
        {
            _catalogContext = context ?? throw new ArgumentNullException(nameof(context));
            _settings = settings.Value;

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        public async Task<IActionResult> Items([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, [FromQuery] string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = await GetItemsByIds(ids);
                if (items.Count == 0)
                    return BadRequest("ids value invalid. Must be comma-separated list of numbers");

                return Ok(items);
            }

            var totalItems = await _catalogContext.CatalogItems
                .LongCountAsync();
            var itemsOnPage = await _catalogContext.CatalogItems
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();
            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CatalogItem>> ItemById(int id)
        {
            if (id <= 0)
                return BadRequest();

            var item = await _catalogContext.CatalogItems.AsNoTracking().SingleOrDefaultAsync(ci => ci.Id == id);
            var baseUri = _settings.PicBaseUrl;

            if (item != null)
            {
                item.PictureUri = baseUri + item.PictureFileName;
                return item;
            }

            return NotFound();
        }

        [HttpGet("name/{name:minlength(1)}")]
        public async Task<ActionResult<PaginatedItemsViewModel<CatalogItem>>> ItemsWithName(string name, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _catalogContext.CatalogItems
                .Where(c => c.Name.StartsWith(name))
                .LongCountAsync();

            var itemsOnPage = await _catalogContext.CatalogItems
                .Where(c => c.Name.StartsWith(name))
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        [HttpGet("type/{catalogTypeId:int}/brand/{catalogBrandId:int?}")]
        public async Task<ActionResult<PaginatedItemsViewModel<CatalogItem>>> ItemsByTypeIdAndBrandId(int catalogTypeId, int? catalogBrandId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var root = _catalogContext.CatalogItems.AsNoTracking();
            root = root.Where(ci => ci.CatalogTypeId == catalogTypeId);

            if (catalogBrandId.HasValue)
                root = root.Where(ci => ci.CatalogBrandId == catalogBrandId);

            var totalItems = await root
                .LongCountAsync();

            var itemsOnPage = await root
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        [HttpGet("type/all/brand/{catalogBrandId:int?}")]
        public async Task<ActionResult<PaginatedItemsViewModel<CatalogItem>>> ItemsByBrandId(int? catalogBrandId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var root = _catalogContext.CatalogItems.AsNoTracking();

            if (catalogBrandId.HasValue)
                root = root.Where(ci => ci.CatalogBrandId == catalogBrandId);

            var totalItems = await root
                .LongCountAsync();

            var itemsOnPage = await root
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<CatalogType>>> CatalogTypes()
        {
            return await _catalogContext.CatalogTypes.AsNoTracking().ToListAsync();
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<CatalogBrand>>> CatalogBrands()
        {
            return await _catalogContext.CatalogBrands.AsNoTracking().ToListAsync();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct([FromBody] CatalogItem productToUpdate)
        {
            var catalogItem = await _catalogContext.CatalogItems.SingleOrDefaultAsync(i => i.Id == productToUpdate.Id);

            if (catalogItem == null)
            {
                return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found." });
            }

            var oldPrice = catalogItem.Price;
            var raiseProductPriceChangedEvent = oldPrice != productToUpdate.Price;

            // Update current product
            catalogItem = productToUpdate;
            _catalogContext.CatalogItems.Update(catalogItem);
            await _catalogContext.SaveChangesAsync();

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

            return CreatedAtAction(nameof(ItemById), new { id = productToUpdate.Id }, null);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] CatalogItem product)
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

            _catalogContext.CatalogItems.Add(item);

            await _catalogContext.SaveChangesAsync();

            return CreatedAtAction(nameof(ItemById), new { id = item.Id }, null);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = _catalogContext.CatalogItems.SingleOrDefault(x => x.Id == id);
            if (product == null)
                return NotFound();

            _catalogContext.CatalogItems.Remove(product);
            await _catalogContext.SaveChangesAsync();

            return NoContent();
        }

        private async Task<List<CatalogItem>> GetItemsByIds(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));
            if (!numIds.All(nid => nid.Ok))
                return [];

            var idsToSelect = numIds
                .Select(id => id.Value);
            var items = await _catalogContext.CatalogItems.AsNoTracking().Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();
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
    }
}
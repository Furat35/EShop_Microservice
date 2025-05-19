using CatalogService.Api.Core.Application.ViewModels;
using CatalogService.Api.Core.Domain;
using CatalogService.Api.Helpers;
using CatalogService.Api.Infrastructure;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CatalogService.Api.Controllers
{
    public class CatalogsController : BaseController<CatalogContext>
    {
        private readonly CatalogSettings _settings;

        public CatalogsController(CatalogContext context, IOptionsSnapshot<CatalogSettings> settings) : base(context)
        {
            _settings = settings.Value;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        public async Task<IActionResult> Items([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10, [FromQuery] string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = await GetItemsByIds(ids);
                if (items.Count == 0)
                    return BadRequest("ids value invalid. Must be comma-separated list of numbers");

                return Ok(items);
            }

            var totalItems = await Context.CatalogItems
                .LongCountAsync();
            var itemsOnPage = await Context.CatalogItems
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .Include(c => c.CatalogType)
                .Include(c => c.CatalogBrand)
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

            var item = await Context.CatalogItems.AsNoTracking().SingleOrDefaultAsync(ci => ci.Id == id);
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
            var totalItems = await Context.CatalogItems
                .Where(c => c.Name.Contains(name))
                .LongCountAsync();

            var itemsOnPage = await Context.CatalogItems
                .Where(c => c.Name.Contains(name))
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .Include(c => c.CatalogType)
                .Include(c => c.CatalogBrand)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            return new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        [HttpGet("type/{catalogTypeId:int}/brand/{catalogBrandId:int?}")]
        public async Task<ActionResult<PaginatedItemsViewModel<CatalogItem>>> ItemsByTypeIdAndBrandId(int catalogTypeId, int? catalogBrandId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var root = Context.CatalogItems.AsNoTracking();
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
            var root = Context.CatalogItems.AsNoTracking();

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
        #region CatalogTypes
        [HttpGet("types")]
        public async Task<ActionResult<List<CatalogType>>> CatalogTypes([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10, [FromQuery] string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = await GetItemsByIds(ids);
                if (items.Count == 0)
                    return BadRequest("ids value invalid. Must be comma-separated list of numbers");

                return Ok(items);
            }

            var totalItems = await Context.CatalogTypes
                .LongCountAsync();
            var itemsOnPage = await Context.CatalogTypes
                .OrderBy(c => c.Type)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var model = new PaginatedItemsViewModel<CatalogType>(pageIndex, pageSize, totalItems, itemsOnPage);
            return Ok(model);
        }

        [HttpGet("types/{type:minlength(1)}")]
        public async Task<ActionResult<List<CatalogType>>> CatalogTypes(string type, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var totalItems = await Context.CatalogTypes
                .Where(ct => ct.Type.Contains(type))
                .LongCountAsync();
            var itemsOnPage = await Context.CatalogTypes
                .Where(ct => ct.Type.Contains(type))
                .OrderBy(c => c.Type)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var model = new PaginatedItemsViewModel<CatalogType>(pageIndex, pageSize, totalItems, itemsOnPage);
            return Ok(model);
        }

        [HttpGet("types/{catalogTypeId:int}")]
        public async Task<ActionResult<CatalogType>> CatalogTypes(int catalogTypeId)
        {
            return await Context.CatalogTypes.FirstOrDefaultAsync(_ => _.Id == catalogTypeId);
        }

        [HttpPost("types")]
        public async Task<ActionResult<CatalogType>> CreateCatalogType([FromBody] CatalogType catalogType)
        {
            await Context.CatalogTypes.AddAsync(catalogType);
            await Context.SaveChangesAsync();

            return Created();
        }

        [HttpPut("types")]
        public async Task<IActionResult> UpdateCatalogType([FromBody] CatalogType catalogType)
        {
            var model = await Context.CatalogTypes.AsNoTracking().FirstOrDefaultAsync(ct => ct.Id == catalogType.Id);
            if (model != null)
                return NotFound();

            Context.CatalogTypes.Update(catalogType);
            await Context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("types/{id}")]
        public async Task<IActionResult> DeleteCatalogType(int id)
        {
            var model = await Context.CatalogBrands.SingleOrDefaultAsync(x => x.Id == id);
            if (model is null)
                return NotFound();

            Context.CatalogBrands.Remove(model);
            await Context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region Catalog Brands
        [HttpGet("brands")]
        public async Task<ActionResult<List<CatalogBrand>>> CatalogBrands([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10, [FromQuery] string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = await GetItemsByIds(ids);
                if (items.Count == 0)
                    return BadRequest("ids value invalid. Must be comma-separated list of numbers");

                return Ok(items);
            }

            var totalItems = await Context.CatalogBrands
                .LongCountAsync();
            var itemsOnPage = await Context.CatalogBrands
                .OrderBy(c => c.Brand)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var model = new PaginatedItemsViewModel<CatalogBrand>(pageIndex, pageSize, totalItems, itemsOnPage);
            return Ok(model);
        }

        [HttpGet("brands/{brand:minlength(1)}")]
        public async Task<ActionResult<List<CatalogBrand>>> CatalogBrands(string brand, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var totalItems = await Context.CatalogBrands
                .Where(ct => ct.Brand.Contains(brand))
                .LongCountAsync();
            var itemsOnPage = await Context.CatalogBrands
                .Where(ct => ct.Brand.Contains(brand))
                .OrderBy(c => c.Brand)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var model = new PaginatedItemsViewModel<CatalogBrand>(pageIndex, pageSize, totalItems, itemsOnPage);
            return Ok(model);
        }


        [HttpGet("brands/{catalogBrandId:int}")]
        public async Task<ActionResult<CatalogBrand>> CatalogBrands(int catalogBrandId)
        {
            return await Context.CatalogBrands.FirstOrDefaultAsync(_ => _.Id == catalogBrandId);
        }

        [HttpPost("brands")]
        public async Task<ActionResult<CatalogBrand>> CreateCatalogBrands([FromBody] CatalogBrand catalogBrand)
        {
            await Context.CatalogBrands.AddAsync(catalogBrand);
            await Context.SaveChangesAsync();

            return Created();
        }

        [HttpPut("brands")]
        public async Task<IActionResult> UpdateCatalogBrands([FromBody] CatalogBrand catalogBrand)
        {
            var model = await Context.CatalogBrands.SingleOrDefaultAsync(x => x.Id == catalogBrand.Id);
            if (model == null)
                return NotFound();

            Context.CatalogBrands.Update(catalogBrand);
            await Context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("brands/{id}")]
        public async Task<IActionResult> DeleteCatalogBrands(int id)
        {
            var model = await Context.CatalogBrands.SingleOrDefaultAsync(x => x.Id == id);
            if (model is null)
                return NotFound();

            Context.CatalogBrands.Remove(model);
            await Context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        [HttpPut]
        public async Task<ActionResult> UpdateProduct([FromBody] CatalogItem productToUpdate)
        {
            var catalogItem = await Context.CatalogItems.SingleOrDefaultAsync(i => i.Id == productToUpdate.Id);

            if (catalogItem == null)
            {
                return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found." });
            }

            var oldPrice = catalogItem.Price;
            var raiseProductPriceChangedEvent = oldPrice != productToUpdate.Price;

            // Update current product
            catalogItem = productToUpdate;
            Context.CatalogItems.Update(catalogItem);
            await Context.SaveChangesAsync();

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

            Context.CatalogItems.Add(item);

            await Context.SaveChangesAsync();

            return CreatedAtAction(nameof(ItemById), new { id = item.Id }, null);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = Context.CatalogItems.SingleOrDefault(x => x.Id == id);
            if (product == null)
                return NotFound();

            Context.CatalogItems.Remove(product);
            await Context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<List<CatalogItem>> GetItemsByIds(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));
            if (!numIds.All(nid => nid.Ok))
                return [];

            var idsToSelect = numIds
                .Select(id => id.Value);
            var items = await Context.CatalogItems.AsNoTracking().Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();
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
using CatalogService.Api.Core.Application.Services;
using CatalogService.Api.Core.Domain;
using CommonLibrary.Controllers;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers
{
    [Authorize]
    public class CatalogsController(IServiceProvider services, ICatalogItemService catalogService,
        ICatalogTypeService catalogTypeService, ICatalogBrandService catalogBrandService) : BaseController<ICatalogItemService>(services)
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Items([FromQuery] PaginationRequestModel request, [FromQuery] string ids = null)
        {
            var response = await catalogService.GetItemsAsync(request.Page, request.PageSize, ids);
            return CreateActionResult(response);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> ItemById(int id)
        {
            var response = await catalogService.GetItemByIdAsync(id);
            return CreateActionResult(response);
        }

        [HttpGet("name/{name:minlength(1)}")]
        [AllowAnonymous]
        public async Task<IActionResult> ItemsWithName(string name, [FromQuery] PaginationRequestModel request)
        {
            var response = await catalogService.ItemsWithNameAsync(name, request.PageSize, request.Page);
            return CreateActionResult(response);
        }

        [HttpGet("type/brand")]
        [AllowAnonymous]
        public async Task<IActionResult> ItemsByTypeIdAndBrandId([FromQuery] int? catalogTypeId, [FromQuery] int? catalogBrandId, [FromQuery] PaginationRequestModel request)
        {
            var response = await catalogService.ItemsByTypeIdAndBrandIdAsync(catalogTypeId, catalogBrandId, request.PageSize, request.Page);
            return CreateActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateItem([FromBody] CatalogItem productToUpdate)
        {
            var response = await catalogService.UpdateItemAsync(productToUpdate);
            return CreateActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] CatalogItem product)
        {
            var response = await catalogService.CreateItemAsync(product);
            return CreateActionResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var response = await catalogService.DeleteItemAsync(id);
            return CreateActionResult(response);
        }

        #region CatalogTypes
        [HttpGet("types")]
        [AllowAnonymous]
        public async Task<IActionResult> CatalogTypes([FromQuery] PaginationRequestModel request, [FromQuery] string ids = null)
        {
            var response = await catalogTypeService.CatalogTypesAsync(request, ids);
            return CreateActionResult(response);
        }

        [HttpGet("types/bytype/{type:minlength(1)}")]
        [AllowAnonymous]
        public async Task<IActionResult> CatalogTypes(string type, [FromQuery] PaginationRequestModel request)
        {
            var response = await catalogTypeService.CatalogTypesAsync(type, request);
            return CreateActionResult(response);
        }

        [HttpGet("types/{catalogTypeId:int}")]
        public async Task<IActionResult> CatalogTypes(int catalogTypeId)
        {
            var response = await catalogTypeService.GetCatalogTypeByIdAsync(catalogTypeId);
            return CreateActionResult(response);
        }

        [HttpPost("types")]
        public async Task<IActionResult> CreateCatalogType([FromBody] CatalogType catalogType)
        {
            var response = await catalogTypeService.CreateCatalogTypeAsync(catalogType);
            return CreateActionResult(response);
        }

        [HttpPut("types")]
        public async Task<IActionResult> UpdateCatalogType([FromBody] CatalogType catalogType)
        {
            var response = await catalogTypeService.UpdateCatalogTypeAsync(catalogType);
            return CreateActionResult(response);
        }

        [HttpDelete("types/{id}")]
        public async Task<IActionResult> DeleteCatalogType(int id)
        {
            var response = await catalogTypeService.DeleteCatalogTypeAsync(id);
            return CreateActionResult(response);
        }
        #endregion

        #region Catalog Brands
        [HttpGet("brands")]
        [AllowAnonymous]
        public async Task<IActionResult> CatalogBrands([FromQuery] PaginationRequestModel request, [FromQuery] string ids = null)
        {
            var response = await catalogBrandService.CatalogBrandsAsync(request, ids);
            return CreateActionResult(response);
        }

        [HttpGet("brands/bybrand/{brand:minlength(1)}")]
        [AllowAnonymous]
        public async Task<IActionResult> CatalogBrands(string brand, [FromQuery] PaginationRequestModel request)
        {
            var response = await catalogBrandService.CatalogBrandsAsync(brand, request);
            return CreateActionResult(response);
        }


        [HttpGet("brands/{catalogBrandId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> CatalogBrands(int catalogBrandId)
        {
            var response = await catalogBrandService.GetCatalogBrandByIdAsync(catalogBrandId);
            return CreateActionResult(response);
        }

        [HttpPost("brands")]
        public async Task<IActionResult> CreateCatalogBrands([FromBody] CatalogBrand catalogBrand)
        {
            var response = await catalogBrandService.CreateCatalogBrandAsync(catalogBrand);
            return CreateActionResult(response);
        }

        [HttpPut("brands")]
        public async Task<IActionResult> UpdateCatalogBrands([FromBody] CatalogBrand catalogBrand)
        {
            var response = await catalogBrandService.UpdateCatalogBrandAsync(catalogBrand);
            return CreateActionResult(response);
        }

        [HttpDelete("brands/{id}")]
        public async Task<IActionResult> DeleteCatalogBrands(int id)
        {
            var response = await catalogBrandService.DeleteCatalogBrandAsync(id);
            return CreateActionResult(response);
        }
        #endregion
    }
}
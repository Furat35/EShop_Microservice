using CatalogService.Api.Core.Application.Services;
using CatalogService.Api.Core.Domain;
using CatalogService.Api.Infrastructure.Context;
using CommonLibrary.Models;
using CommonLibrary.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CatalogService.Api.Infrastructure.Services
{
    public class CatalogBrandService(CatalogContext dbContext)
        : GenericRepository<CatalogContext, CatalogBrand, int>(dbContext), ICatalogBrandService
    {
        public async Task<ResponseDto<PaginatedItemsViewModel<CatalogBrand>>> CatalogBrandsAsync(PaginationRequestModel request, string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = await GetCatalogBrandsByIdsAsync(ids);
                var paginatedItems = new PaginatedItemsViewModel<CatalogBrand>(request.Page, request.PageSize, items.Count, items);
                return ResponseDto<PaginatedItemsViewModel<CatalogBrand>>
                   .GenerateResponse(items.Count > 0)
                   .Success(paginatedItems, HttpStatusCode.OK)
                   .Fail("ids value invalid. Must be comma-separated list of numbers", HttpStatusCode.BadRequest);
            }

            var totalItems = await GetAll().LongCountAsync();
            var itemsOnPage = GetAll();
            itemsOnPage = itemsOnPage.OrderBy(c => c.Brand);

            if (request.PageSize != 0)
                itemsOnPage = itemsOnPage.Skip(request.PageSize * request.Page).Take(request.PageSize);

            var model = new PaginatedItemsViewModel<CatalogBrand>(request.Page, request.PageSize, totalItems, itemsOnPage);
            return ResponseDto<PaginatedItemsViewModel<CatalogBrand>>.Success(model, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<PaginatedItemsViewModel<CatalogBrand>>> CatalogBrandsAsync(string brand, PaginationRequestModel request)
        {
            var totalItems = await GetAll()
                .Where(ct => ct.Brand.Contains(brand))
                .LongCountAsync();


            var itemsOnPage = GetAll().Where(ct => ct.Brand.Contains(brand));
            itemsOnPage= itemsOnPage.OrderBy(c => c.Brand);

            if (request.PageSize != 0)
                itemsOnPage = itemsOnPage.Skip(request.PageSize * request.Page).Take(request.PageSize);

            var model = new PaginatedItemsViewModel<CatalogBrand>(request.Page, request.PageSize, totalItems, itemsOnPage);
            return ResponseDto<PaginatedItemsViewModel<CatalogBrand>>.Success(model, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<CatalogBrand>> GetCatalogBrandByIdAsync(int catalogBrandId)
        {
            var CatalogBrand = await GetByIdAsync(catalogBrandId, false);
            return ResponseDto<CatalogBrand>.Success(CatalogBrand, HttpStatusCode.OK);

        }

        public async Task<ResponseDto<bool>> CreateCatalogBrandAsync(CatalogBrand catalogBrand)
        {
            await AddAsync(catalogBrand);
            return ResponseDto<bool>.GenerateResponse(await SaveChangesAsync() > 0)
                .Success(true, HttpStatusCode.OK)
                .Fail("Error occured while creating catalog brand!", HttpStatusCode.InternalServerError);
        }

        public async Task<ResponseDto<bool>> UpdateCatalogBrandAsync(CatalogBrand catalogBrand)
        {
            var model = await GetByIdAsync(catalogBrand.Id, false);
            if (model is null)
                return ResponseDto<bool>.Fail("Catalog brand is not found!", HttpStatusCode.NotFound);

            Update(catalogBrand);
            return ResponseDto<bool>.GenerateResponse(await SaveChangesAsync() > 0)
              .Success(true, HttpStatusCode.OK)
              .Fail("Error occured while updating catalog brand!", HttpStatusCode.InternalServerError);
        }

        public async Task<ResponseDto<bool>> DeleteCatalogBrandAsync(int id)
        {
            var model = await GetByIdAsync(id, false);
            if (model is null)
                return ResponseDto<bool>.Fail("Catalog brand is not found!", HttpStatusCode.NotFound);

            await DeleteByIdAsync(id);
            return ResponseDto<bool>.GenerateResponse(await SaveChangesAsync() > 0)
              .Success(true, HttpStatusCode.OK)
              .Fail("Error occured while deleting catalog brand!", HttpStatusCode.InternalServerError);
        }

        private async Task<List<CatalogBrand>> GetCatalogBrandsByIdsAsync(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));
            if (!numIds.All(nid => nid.Ok))
                return [];

            var idsToSelect = numIds
                .Select(id => id.Value);
            var items = await GetAll().Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();

            return items;
        }
    }
}

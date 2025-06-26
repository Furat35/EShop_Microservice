using CatalogService.Api.Core.Application.Services;
using CatalogService.Api.Core.Domain;
using CatalogService.Api.Infrastructure.Context;
using CommonLibrary.Models;
using CommonLibrary.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;

namespace CatalogService.Api.Infrastructure.Services
{
    public class CatalogTypeService(CatalogContext dbContext)
        : GenericRepository<CatalogContext, CatalogType, int>(dbContext), ICatalogTypeService
    {
        public async Task<ResponseDto<PaginatedItemsViewModel<CatalogType>>> CatalogTypesAsync(PaginationRequestModel request, string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = await GetCatalogTypesByIdsAsync(ids);
                var paginatedItems = new PaginatedItemsViewModel<CatalogType>(request.Page, request.PageSize, items.Count, items);
                return ResponseDto<PaginatedItemsViewModel<CatalogType>>
                   .GenerateResponse(items.Count > 0)
                   .Success(paginatedItems, HttpStatusCode.OK)
                   .Fail("ids value invalid. Must be comma-separated list of numbers", HttpStatusCode.BadRequest);
            }

            var totalItems = await GetAll()
                .LongCountAsync();

            var itemsOnPage = GetAll();
            itemsOnPage = itemsOnPage.OrderBy(c => c.Type);

            if(request.PageSize == 0)
                itemsOnPage = itemsOnPage.Skip(request.PageSize * request.Page).Take(request.PageSize);

            var model = new PaginatedItemsViewModel<CatalogType>(request.Page, request.PageSize, totalItems, itemsOnPage);
            return ResponseDto<PaginatedItemsViewModel<CatalogType>>.Success(model, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<PaginatedItemsViewModel<CatalogType>>> CatalogTypesAsync(string type, PaginationRequestModel request)
        {
            var totalItems = await GetAll()
                .Where(ct => ct.Type.Contains(type))
                .LongCountAsync();
            var itemsOnPage = GetAll().Where(ct => ct.Type.Contains(type));
            itemsOnPage = itemsOnPage.OrderBy(c => c.Type);
                
            if (request.PageSize == 0)
                itemsOnPage = itemsOnPage.Skip(request.PageSize * request.Page).Take(request.PageSize);

            var model = new PaginatedItemsViewModel<CatalogType>(request.Page, request.PageSize, totalItems, itemsOnPage);
            return ResponseDto<PaginatedItemsViewModel<CatalogType>>.Success(model, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<CatalogType>> GetCatalogTypeByIdAsync(int catalogTypeId)
        {
            var catalogType = await GetByIdAsync(catalogTypeId);
            return ResponseDto<CatalogType>.GenerateResponse(catalogType != null)
                      .Success(catalogType, HttpStatusCode.OK)
                      .Fail("Catalog type not found!", HttpStatusCode.NotFound);
        }

        public async Task<ResponseDto<bool>> CreateCatalogTypeAsync(CatalogType catalogType)
        {
            await AddAsync(catalogType);
            return ResponseDto<bool>.GenerateResponse(await SaveChangesAsync() > 0)
                .Success(true, HttpStatusCode.OK)
                .Fail("Error occured while creating catalog type!", HttpStatusCode.InternalServerError);
        }

        public async Task<ResponseDto<bool>> UpdateCatalogTypeAsync(CatalogType catalogType)
        {
            var model = await GetByIdAsync(catalogType.Id, false);
            if (model is null)
                return ResponseDto<bool>.Fail("Catalog type not found!", HttpStatusCode.NotFound);

            Update(catalogType);
            return ResponseDto<bool>.GenerateResponse(await SaveChangesAsync() > 0)
              .Success(true, HttpStatusCode.OK)
              .Fail("Error occured while creating catalog type!", HttpStatusCode.InternalServerError);
        }

        public async Task<ResponseDto<bool>> DeleteCatalogTypeAsync(int id)
        {
            var model = await GetByIdAsync(id, false);
            if (model is null)
                return ResponseDto<bool>.Fail("Catalog type not found!", HttpStatusCode.NotFound);

            await DeleteByIdAsync(id);
            return ResponseDto<bool>.GenerateResponse(await SaveChangesAsync() > 0)
              .Success(true, HttpStatusCode.OK)
              .Fail("Error occured while deleting catalog type!", HttpStatusCode.InternalServerError);
        }

        private async Task<List<CatalogType>> GetCatalogTypesByIdsAsync(string ids)
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

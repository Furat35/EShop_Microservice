using CatalogService.Api.Core.Domain;
using CommonLibrary.Models;
using CommonLibrary.Repositories.Interfaces;

namespace CatalogService.Api.Core.Application.Services
{
    public interface ICatalogItemService : IGenericRepository<CatalogItem, int>
    {
        Task<ResponseDto<PaginatedItemsViewModel<CatalogItem>>> GetItemsAsync(int page = 0, int pageSize = 10, string ids = null);
        Task<ResponseDto<CatalogItem>> GetItemByIdAsync(int id);
        Task<ResponseDto<PaginatedItemsViewModel<CatalogItem>>> ItemsWithNameAsync(string name, int pageSize = 10, int page = 0);
        Task<ResponseDto<PaginatedItemsViewModel<CatalogItem>>> ItemsByTypeIdAndBrandIdAsync(int? catalogTypeId, int? catalogBrandId, int pageSize = 10, int page = 0);
        Task<ResponseDto<bool>> UpdateItemAsync(CatalogItem productToUpdate);
        Task<ResponseDto<bool>> CreateItemAsync(CatalogItem product);
        Task<ResponseDto<bool>> DeleteItemAsync(int id);
    }
}

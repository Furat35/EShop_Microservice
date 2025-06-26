using CatalogService.Api.Core.Domain;
using CommonLibrary.Models;
using CommonLibrary.Repositories.Interfaces;

namespace CatalogService.Api.Core.Application.Services
{
    public interface ICatalogTypeService : IGenericRepository<CatalogType, int>
    {
        Task<ResponseDto<PaginatedItemsViewModel<CatalogType>>> CatalogTypesAsync(PaginationRequestModel request, string ids = null);
        Task<ResponseDto<PaginatedItemsViewModel<CatalogType>>> CatalogTypesAsync(string type, PaginationRequestModel request);
        Task<ResponseDto<CatalogType>> GetCatalogTypeByIdAsync(int catalogTypeId);
        Task<ResponseDto<bool>> CreateCatalogTypeAsync(CatalogType catalogType);
        Task<ResponseDto<bool>> UpdateCatalogTypeAsync(CatalogType catalogType);
        Task<ResponseDto<bool>> DeleteCatalogTypeAsync(int id);
    }
}

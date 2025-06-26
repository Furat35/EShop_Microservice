using CatalogService.Api.Core.Domain;
using CommonLibrary.Models;
using CommonLibrary.Repositories.Interfaces;

namespace CatalogService.Api.Core.Application.Services
{
    public interface ICatalogBrandService : IGenericRepository<CatalogBrand, int>
    {
        Task<ResponseDto<PaginatedItemsViewModel<CatalogBrand>>> CatalogBrandsAsync(PaginationRequestModel request, string ids);
        Task<ResponseDto<PaginatedItemsViewModel<CatalogBrand>>> CatalogBrandsAsync(string brand, PaginationRequestModel request);
        Task<ResponseDto<CatalogBrand>> GetCatalogBrandByIdAsync(int catalogBrandId);
        Task<ResponseDto<bool>> CreateCatalogBrandAsync(CatalogBrand catalogBrand);
        Task<ResponseDto<bool>> UpdateCatalogBrandAsync(CatalogBrand catalogBrand);
        Task<ResponseDto<bool>> DeleteCatalogBrandAsync(int id);

    }
}

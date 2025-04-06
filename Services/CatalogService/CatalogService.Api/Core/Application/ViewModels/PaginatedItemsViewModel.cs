namespace CatalogService.Api.Core.Application.ViewModels
{
    public class PaginatedItemsViewModel<TEntity>
        (int pagination, int pageSize, long count, IEnumerable<TEntity> data) where TEntity : class
    {
        public int Pagination { get; } = pagination;
        public int PageSize { get; } = pageSize;
        public long Count { get; } = count;
        public IEnumerable<TEntity> Data { get; } = data;
    }
}

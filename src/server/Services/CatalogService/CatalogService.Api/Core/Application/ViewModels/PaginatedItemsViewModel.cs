namespace CatalogService.Api.Core.Application.ViewModels
{
    public class PaginatedItemsViewModel<TEntity>
        (int pageIndex, int pageSize, long count, IEnumerable<TEntity> data) where TEntity : class
    {
        public int PageIndex { get; } = pageIndex;
        public int PageSize { get; } = pageSize;
        public long Count { get; } = count;
        public bool HasNext { get => pageIndex >= 0 && count > (pageIndex + 1) * PageSize; }
        public bool HasPrevious { get => pageIndex - 1 >= 0; }
        //public int TotalPages { get => (int)Math.Ceiling((double)Count / PageSize); }
        public IEnumerable<TEntity> Data { get; } = data;
    }
}

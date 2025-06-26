namespace CommonLibrary.Models
{
    public class PaginatedItemsViewModel<TEntity>
        (int page, int pageSize, long count, IEnumerable<TEntity> data) where TEntity : class
    {
        public int Page { get; } = page;
        public int PageSize { get; } = pageSize;
        public long Count { get; } = count;
        public bool HasNext { get => page >= 0 && count > (page + 1) * PageSize; }
        public bool HasPrevious { get => page - 1 >= 0; }
        public IEnumerable<TEntity> Data { get; } = data;
    }
}

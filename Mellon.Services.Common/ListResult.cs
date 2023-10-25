namespace Mellon.Common.Services
{
    public class ListResult<T>
    {
        public IEnumerable<T> Data { get; }

        public ListResult(IEnumerable<T> items)
        {
            Data = items ?? Enumerable.Empty<T>();
        }

        public ListResult(T singleItem)
        {
            if (singleItem is null)
                Data = Enumerable.Empty<T>();
            else
                Data = Enumerable.Repeat(singleItem, 1);
        }
    }

    public class PartialListResult<T> : ListResult<T>
    {
        public bool HasMore { get; }
        public PartialListResult(IEnumerable<T> items, bool hasMore) : base(items)
        {
            HasMore = hasMore;
        }
    }
}

namespace HSS.System.V2.Domain.Helpers.Models
{
    public class PagedResult<T>(IEnumerable<T> items, int totalCount, int page, int pageSize)
    {
        public IEnumerable<T> Items { get; set; } = items;
        public int TotalCount { get; set; } = totalCount;
        public int PageSize { get; set; } = pageSize;
        public int CurrentPage { get; set; } = page;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public static PagedResult<T> Empty => new([], 0, 1, 1);
    }

}

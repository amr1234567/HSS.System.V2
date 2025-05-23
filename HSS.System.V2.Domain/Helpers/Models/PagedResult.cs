using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HSS.System.V2.Domain.Helpers.Models
{
    public class PagedResult<T>
    {
        public PagedResult(IEnumerable<T> items, int totalCount, int page, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            CurrentPage = page;
            PageSize = pageSize;
        }
        public PagedResult(IEnumerable<T> items, PagedResult<object> result)
        {
            Items = items;
            TotalCount = result.TotalCount;
            CurrentPage = result.CurrentPage;
            PageSize = result.PageSize;
        }
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public static PagedResult<T> Empty => new([], 0, 1, 1);
    }

}

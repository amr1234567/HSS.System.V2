using HSS.System.V2.Domain.Helpers.Models;

using Microsoft.EntityFrameworkCore;

namespace HSS.System.V2.Domain.Helpers.Methods
{
    public static class PaginationServices
    {
        public static PagedResult<T> GetPaged<T>(
            this IEnumerable<T> query, int page, int pageSize) where T : class
        {
            var totalCount = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<T>(items, totalCount, page, pageSize);
        }

        public static PagedResult<T> GetPaged<T>(
           this IEnumerable<T> query, PaginationRequest pagination) where T : class
        {
            var page = pagination.Page;
            var pageSize = pagination.Size;
            var totalCount = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<T>(items, totalCount, page, pageSize);
        }

        public static PagedResult<T> GetPagedAsync<T>(
            this List<T> query, int page, int pageSize) where T : class
        {
            var totalCount = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<T>(items, totalCount, page, pageSize);
        }

        public static async Task<PagedResult<T>> GetPagedAsync<T>(
        this IQueryable<T> query, int page = 1, int pageSize = 10) where T : class
        {
            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PagedResult<T>(items, totalCount, page, pageSize);
        }

        public static async Task<PagedResult<T>> GetPagedAsync<T>(
        this IQueryable<T> query, PaginationRequest pagination) where T : class
        {
            var page = pagination.Page;
            var pageSize = pagination.Size;
            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PagedResult<T>(items, totalCount, page, pageSize);
        }
    }
}

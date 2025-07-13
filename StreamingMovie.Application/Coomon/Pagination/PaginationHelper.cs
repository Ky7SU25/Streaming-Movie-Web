using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace StreamingMovie.Application.Coomon.Pagination
{
    public static class PaginationHelper
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, int page, int pageSize)
        {
            var totalItems = query.Count();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}

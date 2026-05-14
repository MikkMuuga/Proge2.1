using Microsoft.EntityFrameworkCore;
using Proge2._1.Data;

namespace Proge2._1.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query, int page, int pageSize)
        {
            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<T>
            {
                Results = items,
                TotalCount = total,
                CurrentPage = page,
                PageSize = pageSize,
                PageCount = pageSize == 0 ? 0 : (int)Math.Ceiling((double)total / pageSize)
            };
        }
    }
}
using Microsoft.EntityFrameworkCore;

namespace Proge2._1.Data

{
    // Renamed the class to avoid conflict
    public static class PagingExtensionsV2
    {
        // Renamed the method to avoid conflict
        public static async Task<PagedResult<T>> GetPagedAsyncV2<T>(this IQueryable<T> query, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = await query.CountAsync(cancellationToken)
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;

            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);

            return result;
        }
    }
}
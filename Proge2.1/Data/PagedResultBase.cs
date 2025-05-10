using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Proge2._1.Data
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
    }
    public class PagedResult<T> : PagedResultBase
    {
        public IList<T> Results { get; set; } = new List<T>();
        public IEnumerable<Budget> Items { get; internal set; }
        public int TotalItems { get; internal set; }
    }


    // Fix the paging extensions
    public static class PagingExtensions
    {
        public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageNumber = page,  // Set both for compatibility
                PageSize = pageSize
            };

            // Get total count once
            var totalCount = await query.CountAsync(cancellationToken);
            result.RowCount = totalCount;
            result.TotalCount = totalCount;

            // Calculate page count
            var pageCount = (double)totalCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            // Skip and take
            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip)
                                         .Take(pageSize)
                                         .ToListAsync(cancellationToken);

            return result;
        }
    }
}



using Microsoft.EntityFrameworkCore;
using Proge2._1.Data;
using Proge2._1.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proge2._1.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Comment>> GetPagedComments(int page, int pageSize)
        {
            var query = _context.Comments.AsQueryable();

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Comment>
            {
                Results = items,
                TotalCount = totalCount,  // or RowCount
                CurrentPage = page,
                PageSize = pageSize,
                PageCount = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }

        public async Task<Comment> GetCommentById(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateComment(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteComment(int id)
        {
            var comment = await GetCommentById(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CommentExists(int id)
        {
            return await _context.Comments.AnyAsync(e => e.CommentId == id);
        }
    }
}
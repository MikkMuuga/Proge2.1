using Microsoft.EntityFrameworkCore;
using Proge2._1.Data;
using Proge2._1.Data.Repositories;
using Proge2._1.Extensions;
using Proge2._1.Models;
using Proge2._1.Search;
using Proge2._1.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proge2._1.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        public CommentService(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<PagedResult<Comment>> List(int page, int pageSize, CommentSearch search = null)
        {
            search ??= new CommentSearch();

            var query = _context.Comments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search.Content))
                query = query.Where(c => c.Content.Contains(search.Content));

            if (!string.IsNullOrWhiteSpace(search.User))
                query = query.Where(c => c.User.Contains(search.User));

            return await query.OrderByDescending(c => c.Id).GetPagedAsync(page, pageSize);
        }

        public async Task<PagedResult<Comment>> GetPagedComments(int page, int pageSize)
        {
            return await List(page, pageSize);
        }

        public async Task<PagedResult<Comment>> GetPagedComments(int page, int pageSize, CommentSearch search)
        {
            return await List(page, pageSize, search);
        }

        public async Task<Comment> GetCommentById(int id)
        {
            return await _unitOfWork.CommentRepository.GetByIdAsync(id);
        }

        public async Task AddComment(Comment comment)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.CommentRepository.AddAsync(comment);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateComment(Comment comment)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.CommentRepository.UpdateAsync(comment);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteComment(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.CommentRepository.DeleteAsync(id);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> CommentExists(int id)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id);
            return comment != null;
        }
        public async Task Save(Comment comment)
        {
            if (comment.Id == 0)
                await AddComment(comment);
            else
                await UpdateComment(comment);
        }
    }
}
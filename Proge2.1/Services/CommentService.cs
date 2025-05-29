using Proge2._1.Models;
using Proge2._1.Data;
using Proge2._1.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace Proge2._1.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<Comment>> GetPagedComments(int page, int pageSize)
        {
            var allComments = await _unitOfWork.CommentRepository.GetAllAsync();
            var paged = allComments
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<Comment>
            {
                Results = paged,
                TotalCount = allComments.Count(),
                CurrentPage = page,
                PageSize = pageSize,
                PageCount = (int)Math.Ceiling((double)allComments.Count() / pageSize)
            };
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
    }
}

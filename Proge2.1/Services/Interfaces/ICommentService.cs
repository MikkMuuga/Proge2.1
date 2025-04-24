
using Proge2._1.Data;
using Proge2._1.Services;
using System.Threading.Tasks;

namespace Proge2._1.Services.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> GetCommentById(int id);
        Task<PagedResult<Comment>> GetPagedComments(int page, int pageSize);
        Task AddComment(Comment comment);
        Task UpdateComment(Comment comment);
        Task DeleteComment(int id);
        Task<bool> CommentExists(int id);
    }
}
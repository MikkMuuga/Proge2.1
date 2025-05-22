namespace Proge2._1.Data.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }

}

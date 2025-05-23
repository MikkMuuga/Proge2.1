namespace Proge2._1.Data.Repositories
{
    public interface IUnitOfWork
    {
        Task BeginTransaction();
        Task Commit();
        Task Rollback();
        Task SaveAsync();
        Task BeginTransactionAsync();
        Task RollbackAsync();
        Task CommitAsync();

        IBudgetRepository BudgetRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IMaterialsRepository MaterialsRepository { get; }
        IServicessRepository ServicesRepository { get; }
        IMachinesRepository MachinesRepository { get; }
        ICommentRepository CommentRepository { get; }
    }
}

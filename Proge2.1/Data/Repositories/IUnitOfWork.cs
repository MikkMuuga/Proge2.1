namespace Proge2._1.Data.Repositories
{
    public interface IUnitOfWork
    {
        Task BeginTransaction();
        Task Commit();
        Task Rollback();

        public IBudgetRepository Budgets { get; }

    }
}

using Proge2._1.Data.Repositories;

namespace Proge2._1.Data.Repositories;

public class BudgetRepository : BaseRepository<Budget>, IBudgetRepository
{
    public BudgetRepository(ApplicationDbContext context) : base(context)
    {
    }

    // Add custom methods if needed
}

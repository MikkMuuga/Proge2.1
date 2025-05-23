using Proge2._1.Data.Repositories;
namespace Proge2._1.Data.Repositories
{
    public interface IBudgetRepository
    {
        Task AddBudgetAsync(Budget budget);
        Task<Budget> GetBudgetByIdAsync(int id);
        Task<IEnumerable<Budget>> GetBudgetsAsync();
        Task UpdateBudgetAsync(Budget budget);
        Task DeleteBudgetAsync(int id);
        Task<IEnumerable<Budget>> GetAllBudgetsAsync();
    }
}
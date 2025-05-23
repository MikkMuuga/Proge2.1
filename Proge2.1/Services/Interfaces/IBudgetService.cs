using Proge2._1.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proge2._1.Services.Interfaces
{
    public interface IBudgetService
    {
        Task<IEnumerable<Budget>> GetAllBudgetsAsync();
        Task<Budget?> GetBudgetByIdAsync(int id);
        Task AddBudgetAsync(Budget budget);
        Task UpdateBudgetAsync(Budget budget);
        Task DeleteBudgetAsync(int id);
        decimal CalculateTotalCost(Budget budget);
        void UpdateBudget(Budget budget);
        string? GetBudgetById(int value);
        void DeleteBudget(int id);
        void AddBudget(Budget budget);
    }
}


using Proge2._1.Data;
using System.Collections.Generic;

namespace Proge2._1.Services.Interfaces
{
    public interface IBudgetService
    {
        IEnumerable<Budget> GetAllBudgets();
        Budget GetBudgetById(int id);
        void AddBudget(Budget budget);
        void UpdateBudget(Budget budget);
        void DeleteBudget(int id);
        decimal CalculateTotalCost(Budget budget);
    }
}

using Proge2._1.Data;
using Proge2._1.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Proge2._1.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly List<Budget> _budgets = new List<Budget>();
        private int _nextId = 1;

        public IEnumerable<Budget> GetAllBudgets()
        {
            return _budgets;
        }

        public Budget GetBudgetById(int id)
        {
            return _budgets.FirstOrDefault(b => b.BudgetId == id);
        }

        public void AddBudget(Budget budget)
        {
            budget.BudgetId = _nextId++;
            _budgets.Add(budget);
        }

        public void UpdateBudget(Budget budget)
        {
            var existing = GetBudgetById(budget.BudgetId);
            if (existing != null)
            {
                existing.Client = budget.Client;
                existing.Date = budget.Date;
                existing.ServiceCost = budget.ServiceCost;
                existing.TotalCost = CalculateTotalCost(budget.ServiceCost);
            }
        }

        public void DeleteBudget(int id)
        {
            var budget = GetBudgetById(id);
            if (budget != null)
            {
                _budgets.Remove(budget);
            }
        }

        public decimal CalculateTotalCost(decimal serviceCost)
        {
            return serviceCost * 1.2m; // 20% käibemaks
        }
    }
}

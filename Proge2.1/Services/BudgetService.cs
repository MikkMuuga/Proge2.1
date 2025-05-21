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

        private readonly ApplicationDbContext _context;

        public BudgetService(ApplicationDbContext context)
        {
            _context = context;
        }


        public void AddBudget(Budget budget)
        {
            _context.Budgets.Add(budget);
            _context.SaveChanges(); // Persist to DB
        }

        public IEnumerable<Budget> GetAllBudgets()
        {
            return _context.Budgets.ToList();
        }

        public Budget GetBudgetById(int id)
        {
            return _context.Budgets.Find(id);
        }

        public void UpdateBudget(Budget budget)
        {
            _context.Budgets.Update(budget);
            _context.SaveChanges();
        }

        public void DeleteBudget(int id)
        {
            var budget = _context.Budgets.Find(id);
            if (budget != null)
            {
                _context.Budgets.Remove(budget);
                _context.SaveChanges();
            }
        }


        public decimal CalculateTotalCost(decimal serviceCost)
        {
            return serviceCost * 1.2m; // 20% käibemaks
        }
        public decimal CalculateTotalCost(Budget budget)
        {
            return CalculateTotalCost(budget.ServiceCost);
        }

    }
}

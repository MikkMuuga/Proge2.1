using Microsoft.EntityFrameworkCore;
using Proge2._1.Models;

namespace Proge2._1.Data.Repositories
{
    public class BudgetRepository : BaseRepository<Budget>, IBudgetRepository
    {
        private readonly ApplicationDbContext _context;

        public BudgetRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Budget>> GetBudgetsAsync()
        {
            return await _context.Budgets.ToListAsync();
        }

        public async Task<Budget> GetBudgetByIdAsync(int id)
        {
            return await _context.Budgets.FindAsync(id);
        }

        public async Task AddBudgetAsync(Budget budget)
        {
            await _context.Budgets.AddAsync(budget);
        }

        public async Task UpdateBudgetAsync(Budget budget)
        {
            var existing = await _context.Budgets.FindAsync(budget.Id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(budget);
            }
            await Task.CompletedTask;
        }

        public async Task DeleteBudgetAsync(int id)
        {
            var budget = await _context.Budgets.FindAsync(id);
            if (budget != null)
            {
                _context.Budgets.Remove(budget);
            }
        }
        public async Task<IEnumerable<Budget>> GetAllBudgetsAsync()
        {
            return await _context.Budgets.ToListAsync();
        }

    }
}

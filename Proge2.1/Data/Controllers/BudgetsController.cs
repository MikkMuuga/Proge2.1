using Microsoft.AspNetCore.Mvc;
using Proge2._1.Data;
using Proge2._1.Services.Interfaces;
using BudgetModel = Proge2._1.Data.Budget;
using System.Threading.Tasks;

namespace Proge2._1.Controllers
{
    public class BudgetsController : Controller
    {
        private readonly IBudgetService _budgetService;

        public BudgetsController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        // GET: Budgets
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var budgets = _budgetService.GetAllBudgets();

            var pagedBudgets = budgets.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var pagedResult = new PagedResult<Budget>
            {
                Items = pagedBudgets,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = budgets.Count()
            };

            return View(pagedResult); 
        }

        // GET: Budgets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = _budgetService.GetBudgetById(id.Value);
            if (budget == null)
            {
                return NotFound();
            }

            return View(budget);
        }

        // GET: Budgets/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Budget budget)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _budgetService.AddBudget(budget);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving budget: " + ex.Message);
                    return View(budget);
                }
            }
            return View(budget);
        }

        // GET: Budgets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = _budgetService.GetBudgetById(id.Value);
            if (budget == null)
            {
                return NotFound();
            }
            return View(budget);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Budget budget)
        {
            if (id != budget.BudgetId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _budgetService.UpdateBudget(budget);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    if (!BudgetExists(budget.BudgetId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = _budgetService.GetBudgetById(id.Value);
            if (budget == null)
            {
                return NotFound();
            }

            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _budgetService.DeleteBudget(id);
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetExists(int id)
        {
            return _budgetService.GetBudgetById(id) != null;
        }
    }
}
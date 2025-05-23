using Microsoft.AspNetCore.Mvc;
using Proge2._1.Data;
using Proge2._1.Models;
using Proge2._1.Services.Interfaces;
using System;
using System.Linq;
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
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            // Await the async method to get the actual IEnumerable<Budget>
            var budgets = await _budgetService.GetAllBudgetsAsync();

            // Now you can use LINQ methods on the actual collection
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

            var budget = await _budgetService.GetBudgetByIdAsync(id.Value);
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
                    await _budgetService.AddBudgetAsync(budget);
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

            var budget = await _budgetService.GetBudgetByIdAsync(id.Value);
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
                    await _budgetService.UpdateBudgetAsync(budget);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    if (!await BudgetExistsAsync(budget.BudgetId))
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

            var budget = await _budgetService.GetBudgetByIdAsync(id.Value);
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
            await _budgetService.DeleteBudgetAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> BudgetExistsAsync(int id)
        {
            var budget = await _budgetService.GetBudgetByIdAsync(id);
            return budget != null;
        }
    }
}
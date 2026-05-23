using Microsoft.AspNetCore.Mvc;
using Proge2._1.Models;
using Proge2._1.Services.Interfaces;
using Proge2._1.Search;
using Proge2._1.Models; // <-- where BudgetIndexModel lives
using System;
using System.Threading.Tasks;
using Proge2._1.Data;

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
        public async Task<IActionResult> Index(int page = 1, int size = 10, [FromQuery] BudgetSearch search = null)
        {
            var result = await _budgetService.ListAsync(page, size, search ?? new BudgetSearch());

            var model = new BudgetIndexModel
            {
                Budgets = (IEnumerable<Budget>)result.Results,
                TotalItems = result.TotalItems,
                Page = page,
                Size = size,
                Search = search ?? new BudgetSearch()
            };

            return View(model);
        }

        // GET: Budgets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var budget = await _budgetService.GetBudgetByIdAsync(id.Value);
            if (budget == null)
                return NotFound();

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
                }
            }
            return View(budget);
        }

        // GET: Budgets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var budget = await _budgetService.GetBudgetByIdAsync(id.Value);
            if (budget == null)
                return NotFound();

            return View(budget);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Budget budget)
        {
            if (id != budget.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _budgetService.UpdateBudgetAsync(budget);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    if (!await BudgetExistsAsync(budget.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var budget = await _budgetService.GetBudgetByIdAsync(id.Value);
            if (budget == null)
                return NotFound();

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

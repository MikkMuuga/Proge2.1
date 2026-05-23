using Microsoft.EntityFrameworkCore;
using Proge2._1.Data;
using Proge2._1.Data.Repositories;
using Proge2._1.Models;
using Proge2._1.Search;
using Proge2._1.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proge2._1.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BudgetService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddBudgetAsync(Budget budget)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.BudgetRepository.AddBudgetAsync(budget);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Budget>> GetAllBudgetsAsync()
        {
            return await _unitOfWork.BudgetRepository.GetBudgetsAsync();
        }

        public async Task<Budget?> GetBudgetByIdAsync(int id)
        {
            return await _unitOfWork.BudgetRepository.GetBudgetByIdAsync(id);
        }

        public async Task UpdateBudgetAsync(Budget budget)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.BudgetRepository.UpdateBudgetAsync(budget);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteBudgetAsync(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.BudgetRepository.DeleteBudgetAsync(id);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public decimal CalculateTotalCost(Budget budget)
        {
            return budget.ServiceCost * 1.2m; // 20% VAT
        }
        public void UpdateBudget(Budget budget)
        {
            UpdateBudgetAsync(budget).GetAwaiter().GetResult();
        }

        public string? GetBudgetById(int id)
        {

            var budget = GetBudgetByIdAsync(id).GetAwaiter().GetResult();
            return budget?.ToString();
        }

        public void DeleteBudget(int id)
        {
            DeleteBudgetAsync(id).GetAwaiter().GetResult();
        }

        public void AddBudget(Budget budget)
        {
            AddBudgetAsync(budget).GetAwaiter().GetResult();
        }

        public async Task<PagedResult<Budget>> List(int page, int size, BudgetSearch search)
        {
            var budgets = (await _unitOfWork.BudgetRepository.GetBudgetsAsync()).AsQueryable();

            // Apply search filters
            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.Client))
                {
                    budgets = budgets.Where(b => b.Client.Contains(search.Client, StringComparison.OrdinalIgnoreCase));
                }

                if (search.Date.HasValue)
                {
                    budgets = budgets.Where(b => b.Date.Date == search.Date.Value.Date);
                }

                if (search.ServiceCost.HasValue)
                {
                    budgets = budgets.Where(b => b.ServiceCost == search.ServiceCost.Value);
                }

                if (search.TotalCost.HasValue)
                {
                    budgets = budgets.Where(b => b.TotalCost == search.TotalCost.Value);
                }
            }

            // Pagination logic
            var totalItems = budgets.Count();
            var pagedBudgets = budgets
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            return new PagedResult<Budget>
            {
                Items = pagedBudgets,
                TotalItems = totalItems
            };
        }

        public async Task<PagedResult<Budget>> ListAsync(int page, int size, BudgetSearch search)
        {
            return await List(page, size, search);
        }
    }
}

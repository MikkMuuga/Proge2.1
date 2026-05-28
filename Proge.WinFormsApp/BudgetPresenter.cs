using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proge.PublicAPI;

namespace Proge.WinFormsApp
{
    public class BudgetPresenter
    {
        private readonly IApiClient _apiClient;
        private readonly IBudgetView _view;

        public Action<string> OnError { get; set; }

        public BudgetPresenter(IBudgetView view, IApiClient apiClient)
        {
            _apiClient = apiClient;
            _view = view;
            view.Presenter = this;
        }

        public void UpdateView(Budget budget)
        {
            if (budget == null)
            {
                _view.Id = 0;
                _view.Client = string.Empty;
                _view.Date = DateTime.Today;
                _view.ServiceCost = 0;
                _view.TotalCost = 0;
            }
            else
            {
                _view.Id = budget.Id;
                _view.Client = budget.Client;
                _view.Date = budget.Date;
                _view.ServiceCost = budget.ServiceCost;
                _view.TotalCost = budget.TotalCost;
            }
        }

        public async Task Load()
        {
            var result = await _apiClient.List();
            if (!result.IsSuccess)
            {
                OnError?.Invoke(result.ErrorMessage);
                return;
            }
            _view.Budgets = result.Value;
        }

        public async Task Save()
        {
            var budget = _view.SelectedItem ?? new Budget();
            budget.Id = _view.Id;
            budget.Client = _view.Client;
            budget.Date = _view.Date;
            budget.ServiceCost = _view.ServiceCost;
            budget.TotalCost = _view.TotalCost;
            var result = await _apiClient.Save(budget);
            if (!result.IsSuccess)
            {
                OnError?.Invoke(result.ErrorMessage);
                return;
            }
            await Load();
        }

        public async Task Delete()
        {
            if (_view.SelectedItem == null) return;
            var result = await _apiClient.Delete(_view.SelectedItem.Id);
            if (!result.IsSuccess)
            {
                OnError?.Invoke(result.ErrorMessage);
                return;
            }
            await Load();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proge.PublicAPI;

namespace Proge.WinFormsApp
{
    public interface IBudgetView
    {
        IList<Budget> Budgets { get; set; }
        Budget SelectedItem { get; set; }
        string Client { get; set; }
        DateTime Date { get; set; }
        decimal ServiceCost { get; set; }
        decimal TotalCost { get; set; }
        int Id { get; set; }
        BudgetPresenter Presenter { get; set; }
    }
}

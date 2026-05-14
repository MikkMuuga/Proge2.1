using Proge2._1.Data;
using Proge2._1.Models;
using Proge2._1.Search;
using System.Collections.Generic;

namespace Proge2._1.Models
{
    public class BudgetIndexModel
    {
        public IEnumerable<Budget> Budgets { get; set; } = new List<Budget>();

        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalItems { get; set; }

        public BudgetSearch Search { get; set; } = new BudgetSearch();
    }
}

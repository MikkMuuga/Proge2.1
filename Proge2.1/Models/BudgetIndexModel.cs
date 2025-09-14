using Proge2._1.Data;
using Proge2._1.Search;

namespace Proge2._1.Models
{
    public class BudgetIndexModel
    {
        public BudgetSearch Search { get; set; }
        public PagedResult<Budget> Data { get; set; }
    }
}
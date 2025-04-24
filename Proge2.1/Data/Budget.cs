using System.ComponentModel.DataAnnotations;

namespace Proge2._1.Data
{
    public class Budget
    {
        [Required]
        public int BudgetId { get; set; }
        [Required]
        [StringLength(55)]
        public required string Client { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public decimal ServiceCost { get; set; }
        public decimal TotalCost { get; set; }
    }
}

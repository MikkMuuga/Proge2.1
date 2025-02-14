using System.ComponentModel.DataAnnotations;

namespace Proge2._1.Data
{
    public class Budget
    {
        [Required]
        public int id { get; set; }
        [Required]
        [StringLength(55)]
        public string client { get; set; }
        [Required]
        [StringLength(55)]
        public DateTime date { get; set; }
        public decimal ServiceCost { get; set; }
        public decimal TotalCost { get; set; }
    }
}

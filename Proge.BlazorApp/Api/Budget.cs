using System.ComponentModel.DataAnnotations;

namespace Proge.BlazorApp
{
    public class Budget
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Client on kohustuslik")]
        [StringLength(55, ErrorMessage = "Client võib olla maksimaalselt 55 tähemärki")]
        public string Client { get; set; }

        [Required(ErrorMessage = "Kuupäev on kohustuslik")]
        public DateTime Date { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Service Cost peab olema positiivne")]
        public decimal ServiceCost { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total Cost peab olema positiivne")]
        public decimal TotalCost { get; set; }
    }
}
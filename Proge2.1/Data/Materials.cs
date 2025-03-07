using System.ComponentModel.DataAnnotations;

namespace Proge2._1.Data
{
    public class Materials
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(55)]
        public string Unit { get; set; }
        [Required]
        [StringLength(55)]
        public decimal Price { get; set; }

        public string Seller { get; set; }

    }
}

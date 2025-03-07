using System.ComponentModel.DataAnnotations;

namespace Proge2._1.Data
{
    public class Materials
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(55)]
        public required string Unit { get; set; }
        [Required]
        [StringLength(55)]
        public decimal Price { get; set; }

        public required string Seller { get; set; }

    }
}

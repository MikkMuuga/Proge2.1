using System.ComponentModel.DataAnnotations;

namespace Proge2._1.Data
{
    public class Servicess
    {
        [Key] 
        [Required]
        public int ServiceId { get; set; }

        [Required]
        [StringLength(55)]
        public string transportation { get; set; }

        [Required]
        public decimal PanelProduction { get; set; }

        [Required]
        public string montage { get; set; }
    }
}

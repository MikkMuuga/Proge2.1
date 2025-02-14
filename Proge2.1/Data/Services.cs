using System.ComponentModel.DataAnnotations;

namespace Proge2._1.Data
{
    public class Services
    {
        [Required]
        public int Id { get; set; }
        public string transportation { get; set; }
        public decimal PanelProduction { get; set; }
        public string montage { get; set; }
    }
}

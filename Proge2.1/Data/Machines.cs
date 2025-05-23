using System.ComponentModel.DataAnnotations;

namespace Proge2._1.Data
{
    public class Machines: Entity
    {
        [Required]
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(25)]
        public string Workers { get; set; }
        public String Supervision { get; set; }
        public decimal CostOfMachines { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Proge2._1.Data
{
    public class Customer
    {
        public int Id { get; set; }

        
        [Required]
        [StringLength(55)]
        public string Name { get; set; }


        [Required]
        [StringLength(55)]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(55)]
        public required string Contact { get; set; }
    }
}

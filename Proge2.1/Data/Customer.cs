﻿using System.ComponentModel.DataAnnotations;

namespace Proge2._1.Data
{
    public class Customer
    {
        public int CustomerId { get; set; }

        
        [Required]
        [StringLength(55)]
        public string Name { get; set; }
        public DateTime Date { get; set; }

        [Required]
        [StringLength(55)]
        public required string Contact { get; set; }
    }
}

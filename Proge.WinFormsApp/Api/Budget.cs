using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proge.WinFormsApp.Api
{
    public class Budget
    {
        public int Id { get; set; }
        public string Client { get; set; }
        public DateTime Date { get; set; }
        public decimal ServiceCost { get; set; }
        public decimal TotalCost { get; set; }
    }
}

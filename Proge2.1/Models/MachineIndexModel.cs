using Proge2._1.Data;
using Proge2._1.Search;
using System.Collections.Generic;

namespace Proge2._1.Models
{
    public class MachineIndexModel
    {
        public IEnumerable<Machines> Machines { get; set; } = new List<Machines>();
        public MachineSearch Search { get; set; } = new MachineSearch();
        public PagedResult<Machines> Data { get; set; } = new PagedResult<Machines>();
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalItems { get; set; }
    }
}

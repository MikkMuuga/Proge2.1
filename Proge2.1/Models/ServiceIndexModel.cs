using Proge2._1.Data;
using Proge2._1.Search;
using System.Collections.Generic;

namespace Proge2._1.Models
{
    public class ServiceIndexModel
    {
        public IEnumerable<Servicess> Services { get; set; } = new List<Servicess>();
        public ServiceSearch Search { get; set; } = new ServiceSearch();
        public PagedResult<Servicess> Data { get; set; } = new PagedResult<Servicess>();
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalItems { get; set; }
    }
}

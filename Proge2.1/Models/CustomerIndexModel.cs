using Proge2._1.Data;
using Proge2._1.Search;
using System.Collections.Generic;

namespace Proge2._1.Models
{
    public class CustomerIndexModel
    {
        public IEnumerable<Customer> Customers { get; set; } = new List<Customer>();
        public CustomerSearch Search { get; set; } = new CustomerSearch();
        public PagedResult<Customer> Data { get; set; } = new PagedResult<Customer>();
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalItems { get; set; }
    }
}

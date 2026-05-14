using Proge2._1.Data;
using Proge2._1.Search;
using System.Collections.Generic;

namespace Proge2._1.Models
{
    public class MaterialIndexModel
    {
        public IEnumerable<Materials> Materials { get; set; } = new List<Materials>();
        public MaterialSearch Search { get; set; } = new MaterialSearch();
        public PagedResult<Materials> Data { get; set; } = new PagedResult<Materials>();
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalItems { get; set; }
    }
}

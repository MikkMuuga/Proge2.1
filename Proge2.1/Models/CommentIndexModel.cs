using Proge2._1.Search;
using Proge2._1.Data;
using Proge2._1.Models;
using System.Collections.Generic;

namespace Proge2._1.Models
{
    public class CommentIndexModel
    {
        public IEnumerable<Comment> Comments { get; set; } = Enumerable.Empty<Comment>();
        public CommentSearch Search { get; set; } = new CommentSearch();
        public PagedResult<Comment> Data { get; set; } = new PagedResult<Comment>();
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalItems { get; set; }
    }
}

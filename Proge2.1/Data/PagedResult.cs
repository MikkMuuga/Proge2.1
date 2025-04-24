
namespace Proge2._1.Data
{
    public class PagedResults<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; internal set; }
        public int RowCount { get; internal set; }
        public int PageCount { get; internal set; }
        public List<object> Results { get; internal set; }
    }
}

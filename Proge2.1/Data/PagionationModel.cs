namespace Proge2._1.Data
{
    internal class PaginationModel<T>
    {
        public List<Materials> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}

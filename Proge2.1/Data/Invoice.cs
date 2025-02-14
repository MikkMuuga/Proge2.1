namespace Proge2._1.Data
{
    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }

        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public IList<InvoiceLine> Lines { get; set; }

        public Invoice() { }
    }
}

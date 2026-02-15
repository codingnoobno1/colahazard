using LiteDB;

namespace PackTrack.Models
{
    public class Invoice
    {
        [BsonId]
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public List<InvoiceItem> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public string InvoiceNumber => $"INV-{Id:D6}";
    }

    public class InvoiceItem
    {
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
    }
}

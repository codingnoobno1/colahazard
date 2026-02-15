namespace PackTrack.Services
{
    public class OcrService
    {
        public OcrResult ScanImage(string base64Image)
        {
            // MOCK implementation
            // In reality, this would send the image to Tesseract or Azure AI
            
            return new OcrResult
            {
                SupplierName = "Global Plastics Ltd",
                Date = DateTime.Now.AddDays(-2),
                TotalAmount = 12500.50m,
                DetectedItems = new List<OcrItem>
                {
                    new OcrItem { Description = "Thick Bottles Batch A", Quantity = 1000, UnitPrice = 5.00m },
                    new OcrItem { Description = "Shipping Fee", Quantity = 1, UnitPrice = 2500.50m }
                }
            };
        }
    }

    public class OcrResult
    {
        public string SupplierName { get; set; } = "";
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OcrItem> DetectedItems { get; set; } = new();
    }

    public class OcrItem
    {
        public string Description { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

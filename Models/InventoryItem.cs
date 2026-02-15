using LiteDB;

namespace PackTrack.Models
{
    public class InventoryItem
    {
        [BsonId]
        public int Id { get; set; }
        public string BottleType { get; set; } = ""; // Thick, Thin, rPET
        public int Quantity { get; set; }
        public string RackId { get; set; } = "";
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}

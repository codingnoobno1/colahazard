using LiteDB;

namespace PackTrack.Models
{
    public class Rack
    {
        [BsonId]
        public int Id { get; set; }
        public string RackName { get; set; } = string.Empty;
        public int MaxCapacity { get; set; } = 100;
        public int CurrentCount { get; set; } = 0;
        public string BottleType { get; set; } = ""; // Dedicated to specific type?
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}

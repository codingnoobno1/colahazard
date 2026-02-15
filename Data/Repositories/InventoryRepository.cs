using LiteDB;
using PackTrack.Models;

namespace PackTrack.Data.Repositories
{
    public class InventoryRepository
    {
        private readonly LiteDatabase _db;

        public InventoryRepository(LiteDbContext context)
        {
            _db = context.Database;
        }

        public IEnumerable<InventoryItem> GetAll()
        {
            return _db.GetCollection<InventoryItem>("inventory").FindAll();
        }

        public InventoryItem? GetByType(string type)
        {
            return _db.GetCollection<InventoryItem>("inventory").FindOne(x => x.BottleType == type);
        }

        public void Upsert(InventoryItem item)
        {
            var col = _db.GetCollection<InventoryItem>("inventory");
            if (item.Id == 0)
            {
                // check if exists by type
                var existing = col.FindOne(x => x.BottleType == item.BottleType);
                if (existing != null)
                {
                    item.Id = existing.Id;
                }
            }
            col.Upsert(item);
        }
    }
}

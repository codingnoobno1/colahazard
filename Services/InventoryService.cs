using PackTrack.Data.Repositories;
using PackTrack.Models;

namespace PackTrack.Services
{
    public class InventoryService
    {
        private readonly InventoryRepository _repo;

        public InventoryService(InventoryRepository repo)
        {
            _repo = repo;
            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            // Ensure we have records for the 3 types
            var types = new[] { "Thick", "Thin", "rPET" };
            foreach (var type in types)
            {
                if (_repo.GetByType(type) == null)
                {
                    _repo.Upsert(new InventoryItem { BottleType = type, Quantity = 0, LastUpdated = DateTime.Now });
                }
            }
        }

        public int GetStock(string type)
        {
            var item = _repo.GetByType(type);
            return item?.Quantity ?? 0;
        }

        public Dictionary<string, int> GetAllStock()
        {
            var items = _repo.GetAll();
            return items.ToDictionary(k => k.BottleType, v => v.Quantity);
        }

        public void AdjustStock(string type, int change)
        {
            var item = _repo.GetByType(type);
            if (item != null)
            {
                item.Quantity += change;
                if (item.Quantity < 0) item.Quantity = 0;
                item.LastUpdated = DateTime.Now;
                _repo.Upsert(item);
            }
        }

        public void AssignRack(string type, string rackId)
        {
            var item = _repo.GetByType(type);
            if (item != null)
            {
                item.RackId = rackId;
                item.LastUpdated = DateTime.Now;
                _repo.Upsert(item);
            }
        }
    }
}

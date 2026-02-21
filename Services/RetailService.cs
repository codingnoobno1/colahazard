using PackTrack.Data;
using PackTrack.Models;
using System.Linq;

namespace PackTrack.Services
{
    public class RetailService
    {
        private readonly LiteDbContext _db;

        public RetailService(LiteDbContext db)
        {
            _db = db;
        }

        public IEnumerable<RetailLocation> GetAllLocations() => _db.RetailLocations.FindAll();
        public IEnumerable<RetailRack> GetRacksByLocation(int retailId) => _db.RetailRacks.Find(x => x.RetailId == retailId);

        public void RegisterRack(int retailId, string code, int capacity, string bottleType)
        {
            var rack = new RetailRack
            {
                RetailId = retailId,
                RackCode = code,
                CapacityUnits = capacity,
                CurrentUnits = 0,
                BottleTypeAssigned = bottleType,
                LastUpdated = DateTime.Now
            };
            _db.RetailRacks.Insert(rack);
        }

        public void UpdateRackInventory(int rackId, int delta)
        {
            var rack = _db.RetailRacks.FindById(rackId);
            if (rack != null)
            {
                rack.CurrentUnits += delta;
                rack.LastUpdated = DateTime.Now;
                _db.RetailRacks.Update(rack);
            }
        }
    }
}

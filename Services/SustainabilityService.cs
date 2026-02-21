using PackTrack.Data;
using PackTrack.Models;
using System.Linq;

namespace PackTrack.Services
{
    public class SustainabilityService
    {
        private readonly LiteDbContext _db;

        public SustainabilityService(LiteDbContext db)
        {
            _db = db;
            EnsureSeededData();
        }

        private void EnsureSeededData()
        {
            if (!_db.RecoveryCenters.Exists(x => x.Id > 0))
            {
                _db.RecoveryCenters.Insert(new RecoveryCenter { Name = "Green-Cycle Mumbai", Location = "Thane, MH", CapacityPerDay = 10000 });
                _db.RecoveryCenters.Insert(new RecoveryCenter { Name = "Eco-Return Bangalore", Location = "Whitefield, KA", CapacityPerDay = 8000 });
            }
        }

        public IEnumerable<RecoveryCenter> GetAllCenters() => _db.RecoveryCenters.FindAll();
        public IEnumerable<RecyclingEvent> GetAllEvents() => _db.RecyclingEvents.FindAll().OrderByDescending(e => e.ReturnedAt);

        public void LogRecyclingEvent(string bottleId, int centerId, string materialGrade)
        {
            var center = _db.RecoveryCenters.FindById(centerId);
            var result = new RecyclingEvent
            {
                BottleId = bottleId,
                CenterId = centerId,
                ReturnedAt = DateTime.Now,
                MaterialGradeAfterSort = materialGrade,
                CarbonSavedGrams = 5.0, // Avg
                Status = "Verified"
            };
            _db.RecyclingEvents.Insert(result);

            // Update Bottle status
            var bottle = _db.Bottles.FindById(bottleId);
            if (bottle != null)
            {
                bottle.CurrentStatus = "Recycled";
                bottle.CurrentLocationType = "RecoveryCenter";
                bottle.CurrentLocationId = center?.Name ?? "Center-" + centerId;
                _db.Bottles.Update(bottle);

                // Log Movement
                _db.BottleMovements.Insert(new BottleMovement
                {
                    BottleId = bottleId,
                    FromLocationType = "Consumer",
                    ToLocationType = "RecoveryCenter",
                    ToLocationId = bottle.CurrentLocationId,
                    EventType = "Recycled"
                });
            }
        }
    }
}

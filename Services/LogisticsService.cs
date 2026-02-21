using PackTrack.Data;
using PackTrack.Models;
using System.Linq;

namespace PackTrack.Services
{
    public class LogisticsService
    {
        private readonly LiteDbContext _db;

        public LogisticsService(LiteDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Shipment> GetAllShipments() => _db.Shipments.FindAll().OrderByDescending(s => s.PickupDate);
        public IEnumerable<Truck> GetAllTrucks() => _db.Trucks.FindAll();
        
        public Shipment? GetShipmentByBatch(int batchId) => _db.Shipments.FindOne(x => x.BatchId == batchId);

        public void RegisterShipment(int batchId, string truckId, string route)
        {
            var shipment = new Shipment
            {
                BatchId = batchId,
                TruckId = truckId,
                RouteCode = route,
                PickupDate = DateTime.Now,
                DispatchTime = DateTime.Now,
                ExpectedDelivery = DateTime.Now.AddDays(1),
                Status = "InTransit",
                TemperatureLogId = "TEMP-" + Guid.NewGuid().ToString().Substring(0, 5),
                HumidityLogId = "HUM-" + Guid.NewGuid().ToString().Substring(0, 5)
            };
            _db.Shipments.Insert(shipment);

            var truck = _db.Trucks.FindById(truckId);
            if (truck != null)
            {
                truck.Status = "OnRoad";
                _db.Trucks.Update(truck);
            }
        }
    }
}

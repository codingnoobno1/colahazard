using PackTrack.Data.Repositories;
using PackTrack.Models;
using System.Linq;
using PackTrack.Data;

namespace PackTrack.Services
{
    public class FactoryService
    {
        private readonly BatchRepository _batchRepo;
        private readonly LiteDbContext _db;

        public FactoryService(BatchRepository batchRepo, LiteDbContext db)
        {
            _batchRepo = batchRepo;
            _db = db;
            EnsureSeededData();
        }

        private void EnsureSeededData()
        {
            if (!_db.Plants.Exists(x => x.PlantId > 0))
            {
                _db.Plants.Insert(new Plant { PlantCode = "PL-MUM-01", Name = "Mumbai Mega Plant", Location = "Mumbai, MH", Country = "India", Timezone = "IST" });
                _db.Plants.Insert(new Plant { PlantCode = "PL-BLR-02", Name = "Bangalore Eco-Hub", Location = "Bangalore, KA", Country = "India", Timezone = "IST" });
            }

            if (!_db.Trucks.Exists(x => x.TruckId != ""))
            {
                _db.Trucks.Insert(new Truck { TruckId = "T-MH-01-7777", TruckNumber = "MH 01 AB 7777", GPSDeviceId = "GPS-777", CapacityUnits = 50000, Status = "Available" });
                _db.Trucks.Insert(new Truck { TruckId = "T-KA-05-8888", TruckNumber = "KA 05 CD 8888", GPSDeviceId = "GPS-888", CapacityUnits = 50000, Status = "Available" });
            }

            if (!_db.RetailLocations.Exists(x => x.Id > 0))
            {
                _db.RetailLocations.Insert(new RetailLocation { Name = "HyperMart Mumbai", City = "Mumbai", Region = "West", Country = "India", Status = "Active" });
                _db.RetailLocations.Insert(new RetailLocation { Name = "EcoStore Bangalore", City = "Bangalore", Region = "South", Country = "India", Status = "Active" });
                
                // Seed Racks
                _db.RetailRacks.Insert(new RetailRack { RetailId = 1, RackCode = "R-A1", CapacityUnits = 100, CurrentUnits = 45, BottleTypeAssigned = "Thick", LastUpdated = DateTime.Now });
                _db.RetailRacks.Insert(new RetailRack { RetailId = 2, RackCode = "R-B2", CapacityUnits = 150, CurrentUnits = 120, BottleTypeAssigned = "rPET", LastUpdated = DateTime.Now });
            }
        }


        public IEnumerable<ProductionBatch> GetAllBatches() => _batchRepo.GetAll();
        public IEnumerable<Plant> GetAllPlants() => _db.Plants.FindAll();

        public void ProduceBatch(string bottleType, int quantity, string containerType = "Bottle", 
                                 int plantId = 1, string liquidType = "Classic Cola",
                                 double? brixOverride = null, double? phOverride = null, double? co2Override = null)
        {
            var plant = _db.Plants.FindById(plantId);
            
            // 1. Create the Batch
            var batch = new ProductionBatch
            {
                PlantId = plantId,
                BatchCode = $"B-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}",
                BottleType = bottleType,
                ContainerType = containerType,
                BottleMaterialGrade = containerType == "Can" ? "Alu-100" : "rPET-Premium",
                BottleThicknessMicron = containerType == "Can" ? 25 : (bottleType == "Thick" ? 80 : 45),
                CapacityML = 500,
                LiquidType = liquidType,
                LiquidBatchCode = $"LQ-{DateTime.Now:yyyyMMdd}-001",
                ProductionLineId = "LINE-A1",
                MachineId = "MAC-500",
                ShiftCode = "SHIFT-DAY",
                OperatorId = "OP-RAY-22",
                SupervisorId = "SUP-SARAH-99",

                TotalPlannedUnits = quantity,
                TotalProducedUnits = quantity,
                TotalRejectedUnits = (int)(quantity * 0.015),
                Status = "Produced",
                QualityStatus = "Approved",
                WholesaleRate = containerType == "Can" ? 45 : 30,
                MRP = containerType == "Can" ? 75 : 50,
                TaxCode = "GST-18",
                TargetMarket = "Domestic-IN",
                
                BrixLevel = brixOverride ?? (11.2 + (new Random().NextDouble() * 0.4)), 
                AcidityPH = phOverride ?? (2.52 + (new Random().NextDouble() * 0.1)),
                CO2Volumes = co2Override ?? (3.8 + (new Random().NextDouble() * 0.2)),
                IngredientsList = "Carbonated Water, Sugar, Caramel Color, Phosphoric Acid, Caffeine",

                ManufactureDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddMonths(12)
            };
            _batchRepo.Upsert(batch);

            // Register Batch QR
            _db.QrRegistry.Insert(new QrRegistry { QrId = batch.BatchCode, EntityType = "Batch", EntityId = batch.Id.ToString() });

            // 2. Hierarchical Loops: Batch -> Pallets -> Cartons -> Bottles
            int unitsPerPallet = 500;
            int unitsPerCarton = 25; 

            int palletCount = (int)Math.Ceiling((double)quantity / unitsPerPallet);
            
            for (int pIdx = 0; pIdx < palletCount; pIdx++)
            {
                int palletUnits = Math.Min(unitsPerPallet, quantity - (pIdx * unitsPerPallet));
                var pallet = new Pallet
                {
                    BatchId = batch.Id,
                    PalletCode = $"P-{batch.BatchCode}-{pIdx + 1:D2}",
                    TotalUnits = palletUnits,
                    CartonCount = (int)Math.Ceiling((double)palletUnits / unitsPerCarton),
                    NetWeight = palletUnits * (batch.CapacityML / 1000.0),
                    GrossWeight = (palletUnits * (batch.CapacityML / 1000.0)) + 20, // 20kg pallet weight
                    Status = "Packed"
                };
                _db.Pallets.Insert(pallet);
                _db.QrRegistry.Insert(new QrRegistry { QrId = pallet.PalletCode, EntityType = "Pallet", EntityId = pallet.Id.ToString() });

                // 3. Create Cartons within Pallet
                int cartonsInThisPallet = pallet.CartonCount;
                for (int cIdx = 0; cIdx < cartonsInThisPallet; cIdx++)
                {
                    int cartonUnits = Math.Min(unitsPerCarton, palletUnits - (cIdx * unitsPerCarton));
                    var carton = new Carton
                    {
                        BatchId = batch.Id,
                        PalletId = pallet.Id,
                        CartonCode = $"C-{pallet.PalletCode}-{cIdx + 1:D2}",
                        UnitsPerCarton = unitsPerCarton,
                        CurrentUnits = cartonUnits,
                        Weight = (cartonUnits * (batch.CapacityML / 1000.0)) + 0.5, // 0.5kg box
                        Status = "Packed"
                    };
                    _db.Cartons.Insert(carton);
                    _db.QrRegistry.Insert(new QrRegistry { QrId = carton.CartonCode, EntityType = "Carton", EntityId = carton.Id.ToString() });

                    // 4. Create Bottles within Carton
                    for (int bIdx = 0; bIdx < cartonUnits; bIdx++)
                    {
                        var bottle = new BottleUnit
                        {
                            BottleId = $"ECO-{carton.CartonCode}-{bIdx + 1:D3}",
                            BatchId = batch.Id,
                            PalletId = pallet.Id,
                            CartonId = carton.Id,
                            BottleType = batch.BottleType,
                            ThicknessMicron = batch.BottleThicknessMicron,
                            MaterialGrade = batch.BottleMaterialGrade,
                            CapacityML = batch.CapacityML,
                            ContainerType = batch.ContainerType,
                            LiquidType = batch.LiquidType,
                            ManufactureDate = batch.ManufactureDate,
                            ExpiryDate = batch.ExpiryDate,

                            FillTemperature = 4.0 + (new Random().NextDouble() * 1.5),
                            PressureAtSeal = 3.6 + (new Random().NextDouble() * 0.3),
                            CarbonationLevel = 3.8 + (new Random().NextDouble() * 0.2),
                            pHValue = batch.AcidityPH,
                            BrixLevel = batch.BrixLevel,
                            CO2Volumes = batch.CO2Volumes,
                            
                            CapType = "DoubleSeal-TamperEvident",
                            LabelVersion = "v3.0-Industrial",
                            CurrentStatus = "Produced",
                            CurrentLocationType = "Plant",
                            CurrentLocationId = plant?.PlantCode ?? "PL-01",
                            CreatedAt = DateTime.Now
                        };
                        _db.Bottles.Insert(bottle);
                        _db.QrRegistry.Insert(new QrRegistry { QrId = bottle.BottleId, EntityType = "Bottle", EntityId = bottle.BottleId });

                        // Log Movement
                        _db.BottleMovements.Insert(new BottleMovement 
                        { 
                            BottleId = bottle.BottleId, 
                            ToLocationType = "Plant", 
                            ToLocationId = bottle.CurrentLocationId,
                            EventType = "Produced" 
                        });
                    }
                }
            }
        }

        public void UpdateBatchStatus(int batchId, string status)
        {
            var batch = _batchRepo.GetById(batchId);
            if (batch == null) return;

            var oldStatus = batch.Status;
            batch.Status = status;
            _batchRepo.Upsert(batch);

            // Propagate to all child bottles
            var bottles = _db.Bottles.Find(x => x.BatchId == batchId).ToList();
            foreach (var b in bottles)
            {
                var prevStatus = b.CurrentStatus;
                b.CurrentStatus = status;
                
                string toLocType = b.CurrentLocationType;
                string toLocId = b.CurrentLocationId;

                if (status == "Packed") 
                { 
                    toLocType = "Plant"; 
                }
                else if (status == "Shipped") 
                { 
                    toLocType = "Truck"; 
                    toLocId = "T-MH-01-7777";
                }
                else if (status == "Delivered") 
                { 
                    toLocType = "Retail"; 
                    toLocId = "RT-MUM-101";
                }

                b.CurrentLocationType = toLocType;
                b.CurrentLocationId = toLocId;
                _db.Bottles.Update(b);

                _db.BottleMovements.Insert(new BottleMovement
                {
                    BottleId = b.BottleId,
                    FromLocationType = prevStatus,
                    FromLocationId = b.CurrentLocationId, // approximate
                    ToLocationType = toLocType,
                    ToLocationId = toLocId,
                    EventType = status == "Shipped" ? "Shipped" : (status == "Delivered" ? "Retail" : status)
                });
            }

            if (status == "Shipped")
            {
                _db.Shipments.Insert(new Shipment
                {
                    BatchId = batchId,
                    TruckId = "T-MH-01-7777",
                    DriverId = "DR-RAHUL-44",
                    PickupDate = DateTime.Now,
                    DispatchTime = DateTime.Now,
                    ExpectedDelivery = DateTime.Now.AddDays(1),
                    Status = "InTransit",
                    RouteCode = "MUM-PUNE-EXP"
                });
            }
        }

        // Stats & KPI Methods
        public Dictionary<string, int> GetProductionStats() => _batchRepo.GetAll().GroupBy(b => b.ContainerType).ToDictionary(g => g.Key, g => g.Sum(b => b.TotalProducedUnits));
        public decimal GetTotalProfit() => _batchRepo.GetAll().Sum(b => (b.TotalProducedUnits - b.TotalRejectedUnits) * (b.WholesaleRate * 0.45m));
        public int GetTotalProducedToday() => _batchRepo.GetAll().Where(b => b.CreatedAt >= DateTime.Today).Sum(b => b.TotalProducedUnits);
        public double GetRecycleRate()
        {
            var total = _db.Bottles.Count();
            return total == 0 ? 0 : Math.Round(((double)_db.Bottles.Count(x => x.CurrentStatus == "Recycled") / total) * 100, 1);
        }
        public double GetCarbonOffset() => _db.RecyclingEvents.FindAll().Sum(x => x.CarbonSavedGrams);
        public double GetMaterialRecoveredKg() => (_db.Bottles.Count(x => x.CurrentStatus == "Recycled") * 25.0) / 1000.0;
        
        public IEnumerable<RetailRack> GetRacksForRetail(int retailId) => _db.RetailRacks.Find(x => x.RetailId == retailId);
    }
}


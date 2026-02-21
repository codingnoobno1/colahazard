using LiteDB;
using PackTrack.Models;

namespace PackTrack.Data
{
    public class LiteDbContext
    {
        private readonly string _dbPath;

        public LiteDbContext(IConfiguration configuration)
        {
            var dbName = configuration.GetValue<string>("LiteDbOptions:DatabaseLocation") ?? "retailer.db";
            _dbPath = Path.Combine(Directory.GetCurrentDirectory(), dbName);
        }

        private LiteDatabase? _database;
        public LiteDatabase Database 
        {
            get
            {
                if (_database == null)
                {
                    _database = new LiteDatabase($"Filename={_dbPath};Connection=Shared");
                    EnsureIndexes(_database);
                }
                return _database;
            }
        }

        private void EnsureIndexes(LiteDatabase db)
        {
            var bottles = db.GetCollection<BottleUnit>("bottles");
            bottles.EnsureIndex(x => x.BatchId);
            bottles.EnsureIndex(x => x.CurrentStatus);

            var qr = db.GetCollection<QrRegistry>("qr_registry");
            qr.EnsureIndex(x => x.QrId);

            var batches = db.GetCollection<ProductionBatch>("production_batches");
            batches.EnsureIndex(x => x.BatchCode);
            batches.EnsureIndex(x => x.PlantId);

            var pallets = db.GetCollection<Pallet>("pallets");
            pallets.EnsureIndex(x => x.PalletCode);

            var cartons = db.GetCollection<Carton>("cartons");
            cartons.EnsureIndex(x => x.CartonCode);

            var movements = db.GetCollection<BottleMovement>("bottle_movements");
            movements.EnsureIndex(x => x.BottleId);
            movements.EnsureIndex(x => x.Timestamp);

            var shipments = db.GetCollection<Shipment>("shipments");
            shipments.EnsureIndex(x => x.BatchId);
            shipments.EnsureIndex(x => x.TruckId);

            var trucks = db.GetCollection<Truck>("trucks");
            trucks.EnsureIndex(x => x.TruckId); 

            var plants = db.GetCollection<Plant>("plants");
            plants.EnsureIndex(x => x.PlantId);
        }





        public ILiteCollection<InventoryItem> Inventory => Database.GetCollection<InventoryItem>("inventory");
        public ILiteCollection<RetailRack> RetailRacks => Database.GetCollection<RetailRack>("retail_racks");

        public ILiteCollection<Invoice> Invoices => Database.GetCollection<Invoice>("invoices");
        
        // Enterprise Factory Collections
        public ILiteCollection<Plant> Plants => Database.GetCollection<Plant>("plants");
        public ILiteCollection<ProductionBatch> ProductionBatches => Database.GetCollection<ProductionBatch>("production_batches");
        public ILiteCollection<Pallet> Pallets => Database.GetCollection<Pallet>("pallets");
        public ILiteCollection<Carton> Cartons => Database.GetCollection<Carton>("cartons");
        public ILiteCollection<BottleUnit> Bottles => Database.GetCollection<BottleUnit>("bottles");
        public ILiteCollection<Shipment> Shipments => Database.GetCollection<Shipment>("shipments");
        public ILiteCollection<Truck> Trucks => Database.GetCollection<Truck>("trucks");
        public ILiteCollection<RetailLocation> RetailLocations => Database.GetCollection<RetailLocation>("retail_locations");
        public ILiteCollection<RecoveryCenter> RecoveryCenters => Database.GetCollection<RecoveryCenter>("recovery_centers");
        public ILiteCollection<RecyclingEvent> RecyclingEvents => Database.GetCollection<RecyclingEvent>("recycling_events");
        public ILiteCollection<BottleMovement> BottleMovements => Database.GetCollection<BottleMovement>("bottle_movements");
        public ILiteCollection<QrRegistry> QrRegistry => Database.GetCollection<QrRegistry>("qr_registry");
    }
}




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

        public LiteDatabase Database => new LiteDatabase($"Filename={_dbPath};Connection=Shared");

        public ILiteCollection<InventoryItem> Inventory => Database.GetCollection<InventoryItem>("inventory");
        public ILiteCollection<Rack> Racks => Database.GetCollection<Rack>("racks");
        public ILiteCollection<Invoice> Invoices => Database.GetCollection<Invoice>("invoices");
    }
}

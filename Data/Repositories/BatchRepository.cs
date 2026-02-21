using LiteDB;
using PackTrack.Models;

namespace PackTrack.Data.Repositories
{
    public class BatchRepository
    {
        private readonly LiteDatabase _db;

        public BatchRepository(LiteDbContext context)
        {
            _db = context.Database;
        }

        public IEnumerable<ProductionBatch> GetAll()
        {
            return _db.GetCollection<ProductionBatch>("production_batches").FindAll();
        }

        public ProductionBatch? GetById(int id)
        {
            return _db.GetCollection<ProductionBatch>("production_batches").FindById(id);
        }

        public void Upsert(ProductionBatch batch)
        {
            _db.GetCollection<ProductionBatch>("production_batches").Upsert(batch);
        }

        public void Delete(int id)
        {
            _db.GetCollection<ProductionBatch>("production_batches").Delete(id);
        }
    }
}


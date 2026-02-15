using LiteDB;
using PackTrack.Models;

namespace PackTrack.Data.Repositories
{
    public class RackRepository
    {
        private readonly LiteDatabase _db;

        public RackRepository(LiteDbContext context)
        {
            _db = context.Database;
        }

        public IEnumerable<Rack> GetAll()
        {
            return _db.GetCollection<Rack>("racks").FindAll();
        }

        public Rack? GetByName(string name)
        {
            return _db.GetCollection<Rack>("racks").FindOne(x => x.RackName == name);
        }

        public void Upsert(Rack rack)
        {
            _db.GetCollection<Rack>("racks").Upsert(rack);
        }
    }
}

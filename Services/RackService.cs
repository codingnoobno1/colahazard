using PackTrack.Data.Repositories;
using PackTrack.Models;
using System.Linq;

namespace PackTrack.Services
{
    public class RackService
    {
        private readonly RackRepository _repo;

        public RackService(RackRepository repo)
        {
            _repo = repo;
            // Initialize default racks if none exist
            if (!_repo.GetAll().Any())
            {
                _repo.Upsert(new Rack { RackName = "A1", MaxCapacity = 500, BottleType = "Thick" });
                _repo.Upsert(new Rack { RackName = "B1", MaxCapacity = 500, BottleType = "Thin" });
            }
        }

        public IEnumerable<Rack> GetRacks()
        {
            return _repo.GetAll();
        }

        public void CreateRack(string name, int capacity, string type)
        {
            if (_repo.GetByName(name) == null)
            {
                _repo.Upsert(new Rack { RackName = name, MaxCapacity = capacity, BottleType = type });
            }
        }
    }
}

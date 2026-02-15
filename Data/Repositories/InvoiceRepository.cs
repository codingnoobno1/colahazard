using LiteDB;
using PackTrack.Models;

namespace PackTrack.Data.Repositories
{
    public class InvoiceRepository
    {
        private readonly LiteDatabase _db;

        public InvoiceRepository(LiteDbContext context)
        {
            _db = context.Database;
        }

        public IEnumerable<Invoice> GetAll()
        {
            return _db.GetCollection<Invoice>("invoices").FindAll().OrderByDescending(x => x.Date);
        }

        public Invoice? GetById(int id)
        {
            return _db.GetCollection<Invoice>("invoices").FindById(id);
        }

        public void Upsert(Invoice invoice)
        {
            _db.GetCollection<Invoice>("invoices").Upsert(invoice);
        }
    }
}

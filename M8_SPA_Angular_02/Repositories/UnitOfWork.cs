using M8_SPA_Angular_02.Models;
using M8_SPA_Angular_02.Repositories.Interfaces;

namespace M8_SPA_Angular_02.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        ProductDbContext db;
        public UnitOfWork(ProductDbContext db)
        {
            this.db = db;
        }
        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.db.Dispose();
        }

        public IGenericRepository<T> GetRepo<T>() where T : class, new()
        {
            return new GenericRepo<T>(this.db);
        }
    }
}

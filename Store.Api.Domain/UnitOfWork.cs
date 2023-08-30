using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Store.Api.Domain.Data.Entities;

namespace Store.Api.Domain
{
    public class UnitOfWork : IDisposable
    {
        private readonly EntityContext _context;
        private bool disposed = false;

        #region Repositories
        public GenericRepository<Items> ItemsRepository { get; private set; }
        #endregion

        public DatabaseFacade Database => _context.Database;

        public UnitOfWork(EntityContext context)
        {
            _context = context;
            ItemsRepository = new GenericRepository<Items>(_context);
        }

        public EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return _context.Entry<TEntity>(entity);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> ExecuteRawSqlAsync(string sql, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

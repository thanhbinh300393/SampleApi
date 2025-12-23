
using Sample.Common.Domain;
using Sample.Infrastructure.Database;

namespace Sample.Infrastructure.Domain
{
    public class EFUnitOfWork : IEFUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private Dictionary<string, object> _repositories;

        public EFUnitOfWork(
            ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
            _repositories = new Dictionary<string, object>();
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this._dbContext.SaveChangesAsync(cancellationToken);
        }

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                _dbContext.Dispose();
            _disposed = true;
        }

        public void Register<TEntity>(IEFRepository<TEntity> repository) where TEntity : class, IEntity
        {
            var typeName = repository.GetType().FullName;
            if (!_repositories.ContainsKey(typeName))
            {
                _repositories.Add(typeName, repository);
            }
            else
            {
                _repositories[typeName] = repository;
            }
            repository.SetDbContext(_dbContext);
        }

        public IEFRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            string? typeName = typeof(IEFRepository<TEntity>).FullName;
            if (typeName == null)
            {
                throw new Exception("Repository type name could not be determined.");
            }

            if (_repositories.ContainsKey(typeName))
            {
                return (IEFRepository<TEntity>)_repositories[typeName];
            }
            throw new Exception($"{typeName} not found");
        }

    }
}
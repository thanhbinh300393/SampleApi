using Microsoft.EntityFrameworkCore;
using Sample.Common.Domain;
using Sample.Infrastructure.Database;

namespace Sample.Infrastructure
{
    public class GenericEFRepository<TEntity> : IEFRepository<TEntity> where TEntity : class, IEntity
    {
        private ApplicationDbContext? _dbContext;
        private DbSet<TEntity> _dbSet
        {
            get
            {
                if (_dbContext == null)
                    throw new InvalidOperationException("DbContext has not been set.");
                return _dbContext.Set<TEntity>();
            }
        }
        private readonly IEFUnitOfWork _unitOfWork;

        public GenericEFRepository(IEFUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.Register(this);
        }

        public DbSet<TEntity> DbSet { get { return _dbSet; } }

        public void SetDbContext(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<TEntity?> GetAsync(object id)
        {
            return await _dbSet.FindAsync(id).AsTask();
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return;

            _dbSet.Remove(entity);
        }


        public void SetDbContext(object dbContext)
        {
            if (dbContext is not ApplicationDbContext context)
                throw new ArgumentException("dbContext must be of type ApplicationDbContext", nameof(dbContext));
            _dbContext = context;
        }
    }
}

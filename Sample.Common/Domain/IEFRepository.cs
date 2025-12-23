using Microsoft.EntityFrameworkCore;
using Sample.Common.Dependency;

namespace Sample.Common.Domain
{
    public interface IEFRepository<TEntity> : ITransientDependency where TEntity : class, IEntity
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity?> GetAsync(object id);

        void Insert(TEntity entity);

        void Update(TEntity entity);

        Task DeleteAsync(object id);

        void SetDbContext(object dbContext);

        DbSet<TEntity> DbSet { get; }
    }
}

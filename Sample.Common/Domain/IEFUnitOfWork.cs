namespace Sample.Common.Domain
{
    public interface IEFUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken));

        void Register<TEntity>(IEFRepository<TEntity> repository) where TEntity : class, IEntity;

        IEFRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
    }
}
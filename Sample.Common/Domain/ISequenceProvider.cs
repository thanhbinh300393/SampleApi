using Sample.Common.Dependency;
using System;
using System.Threading.Tasks;

namespace Sample.Common.Domain
{
    public interface ISequenceProvider : ISingletonDependency
    {
        Task<int> GetIntValue<TEntity>() where TEntity : class, IEntity;
        Task<long> GetLongValue<TEntity>() where TEntity : class, IEntity;
        Task<Guid> GetGuidValue();
    }
}

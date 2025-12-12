using Sample.Common.FilterList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Common.Domain
{
    public interface IDapperRepository<TEntity> : IRepository where TEntity : class, IEntity
    {
        Task<TResult> SqlHelper<TResult>(Func<IDbConnection, IDbTransaction, Task<TResult>> func);
        Task SqlHelper(Func<IDbConnection, IDbTransaction, Task> func);

        Task<FilterResult<T>> Filter<T>(FilterRequest dataRequest) where T : class;
        Task<FilterResult<TEntity>> Filter(FilterRequest dataRequest);
        Task<List<T>> GetAllAsync<T>(FilterRequest dataRequest) where T : class;
        
        Task<List<T>> GetAllAsync<T>(List<FilterRequest> dataRequest) where T : class;
        Task<List<TEntity>> GetAllAsync(FilterRequest dataRequest);
        
        Task<List<TEntity>> GetAllAsync(List<FilterRequest> dataRequest);
        Task<T?> FirstOrDefaultAsync<T>(FilterRequest dataRequest) where T : class;
        Task<TEntity?> FirstOrDefaultAsync(FilterRequest dataRequest);
        Task<TEntity?> FirstOrDefault(FilterRequest dataRequest);
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetHierarchy();

        Task<TEntity> GetAsync(object id);

        Task<TEntity> InsertOrUpdateAsync(TEntity entity);

        Task<TEntity> InsertAsync(TEntity entity);
        Task BulkInsertAsync(IEnumerable<TEntity> entities);

        Task<TEntity> UpdateAsync(TEntity entity);
        Task BulkUpdateAsync(IEnumerable<TEntity> entities);

        Task<TEntity> DeleteAsync(TEntity entity);
        Task BulkDeleteAsync(IEnumerable<TEntity> entities);
    }
}
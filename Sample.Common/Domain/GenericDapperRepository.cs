using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Sample.Common.Database;
using Sample.Common.Entities;
using Sample.Common.Exceptions;
using Sample.Common.FilterList;
using Sample.Common.Heplers;
using System.Data;
using System.Reflection;

namespace Sample.Common.Domain
{
    public class GenericDapperRepository<TEntity> :  IDapperRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly IDapperUnitOfWork _unitOfWork;
        protected IDbConnection Connection => _unitOfWork.GetConnection<TEntity>();
        protected IDbTransaction Transaction => _unitOfWork.GetTransaction<TEntity>();

        public GenericDapperRepository(IDapperUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Connection extenstion

        public async Task<TResult> SqlHelper<TResult>(Func<IDbConnection, IDbTransaction, Task<TResult>> func)
        {
            return await func(Connection, Transaction);
        }

        public async Task SqlHelper(Func<IDbConnection, IDbTransaction, Task> func)
        {
            await func(Connection, Transaction);
        }

        #endregion


        public virtual async Task<FilterResult<T>> Filter<T>(FilterRequest dataRequest) where T : class
        {
            return await Connection.RunAsync<TEntity, T>(dataRequest, transaction: Transaction);
        }

        public async Task<FilterResult<TEntity>> Filter(FilterRequest dataRequest)
        {
            return await Filter<TEntity>(dataRequest);
        }

        public async Task<List<T>> GetAllAsync<T>(FilterRequest dataRequest) where T : class
        {
            dataRequest.IsFull = true;
            var result = await Filter<T>(dataRequest);
            return result.List ?? new List<T>();
        }

        public async Task<List<T>> GetAllAsync<T>(List<FilterRequest> dataRequest) where T : class
        {
            var response = new List<T>();
            foreach (var filterRequest in dataRequest)
            {
                filterRequest.IsFull = true;
                var result = await Filter<T>(filterRequest);
                response.AddRange(result.List);
            }

            return response;
        }

        public async Task<List<TEntity>> GetAllAsync(FilterRequest dataRequest)
        {
            dataRequest.IsFull = true;
            var result = await Filter<TEntity>(dataRequest);
            return result.List ?? new List<TEntity>();
        }

        public async Task<List<TEntity>> GetAllAsync(List<FilterRequest> dataRequest)
        {
            var response = new List<TEntity>();
            foreach (var filterRequest in dataRequest)
            {
                filterRequest.IsFull = true;
                try
                {
                    var result = await Filter<TEntity>(filterRequest);
                    response.AddRange(result.List);
                }
                catch (Exception)
                {
                }
            }

            return response;
        }

        public async Task<T?> FirstOrDefaultAsync<T>(FilterRequest dataRequest) where T : class
        {
            dataRequest.IsFull = false;
            dataRequest.Limit = 1;
            dataRequest.Page = 1;
            var result = await Filter<T>(dataRequest);

            return result.List.FirstOrDefault();
        }

        public async Task<TEntity?> FirstOrDefaultAsync(FilterRequest dataRequest)
        {
            return await FirstOrDefaultAsync<TEntity>(dataRequest);
        }

        public virtual async Task<TEntity?> FirstOrDefault(FilterRequest dataRequest)
        {
            return await FirstOrDefault<TEntity>(dataRequest);
        }

        public async Task<T?> FirstOrDefault<T>(FilterRequest dataRequest) where T : class
        {
            dataRequest.IsFull = false;
            dataRequest.Limit = 1;
            dataRequest.Page = 1;
            dataRequest.HasGetCount = false;
            var result = await Filter<T>(dataRequest);

            return result.List.FirstOrDefault();
        }

        public async virtual Task<TEntity> DeleteAsync(TEntity entity)
        {
            var keyPrimaryProp = EntityExtension.GetPropertyPrimaryKey<TEntity>();
            if (keyPrimaryProp == null)
                throw new DevelopException($"Entity type {typeof(TEntity).Name} not defined primary key.", new { entity, keyPrimaryProp });

            var id = entity.GetValuePrimaryKey<TEntity>()?.ToString();

            if (id == null || string.IsNullOrWhiteSpace(id))
                throw new DevelopException($"Value primary key is null.", new { entity, keyPrimaryProp });

            var tableName = EntityExtension.GetTableName<TEntity>();
            var cmd = $"DELETE FROM {tableName} WHERE [{keyPrimaryProp.Name}] = '{id.PreventSqlInjection()}';";

            await Connection.ExecuteAsync(cmd, transaction: Transaction);
            return entity;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return Connection.GetAll<TEntity>(transaction: Transaction).AsQueryable();
        }

        public virtual async Task<TEntity> GetAsync(object id)
        {
            return await Connection.GetAsync<TEntity>(id, transaction: Transaction);
        }

        public virtual IQueryable<TEntity> GetHierarchy()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            var keyPrimary = EntityExtension.GetPropertyPrimaryKey<TEntity>();
            if (keyPrimary == null)
                throw new DevelopException($"Entity type {typeof(TEntity).Name} not defined primary key.", new { entity, keyPrimary });
            var id = entity.GetValuePrimaryKey();

            if ((await Connection.GetAsync<TEntity>(id, transaction: Transaction)) == null)
            {
                Connection.Insert(entity, transaction: Transaction);
            }
            else
            {
                Connection.Update(entity, transaction: Transaction);
            }

            return await GetAsync(id);
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            var keyPrimary = EntityExtension.GetPropertyPrimaryKey<TEntity>();
            if (keyPrimary == null)
                throw new DevelopException($"Entity type {typeof(TEntity).Name} not defined primary key.", new { entity, keyPrimary });
            var id = entity.GetValuePrimaryKey();

            Connection.Insert(entity, transaction: Transaction);
            return await GetAsync(id);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var keyPrimary = EntityExtension.GetPropertyPrimaryKey<TEntity>();
            if (keyPrimary == null)
                throw new DevelopException($"Entity type {typeof(TEntity).Name} not defined primary key.", new { entity, keyPrimary });
            var id = entity.GetValuePrimaryKey();

            Connection.Update(entity, transaction: Transaction);
            return await GetAsync(id);
        }

        //public virtual async Task BulkInsertAsync(IEnumerable<TEntity> entities)
        //{
        //    foreach (var item in entities)
        //    {
        //        Connection.Insert(item, transaction: Transaction);
        //    }
        //}

        public virtual async Task BulkInsertAsync(IEnumerable<TEntity> entities)
        {
            var entityList = entities.ToList();
            if (!entityList.Any())
                return;

            using var bulk = new SqlBulkCopy((SqlConnection)Connection,
                SqlBulkCopyOptions.Default,
                (SqlTransaction)Transaction);

            bulk.DestinationTableName = QueriesCreatingHelper.GetTableName<TEntity>();

            var table = new DataTable();
            var properties = typeof(TEntity)
                .GetProperties()
                .Where(p => p.CanRead &&
                            (p.PropertyType.IsValueType || p.PropertyType == typeof(string) || p.PropertyType == typeof(DateTime?)))
                .ToList();

            foreach (var prop in properties)
            {
                var columnType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                table.Columns.Add(prop.Name, columnType);
                bulk.ColumnMappings.Add(prop.Name, prop.Name);
            }

            foreach (var item in entityList)
            {
                var values = properties.Select(p => p.GetValue(item) ?? DBNull.Value).ToArray();
                table.Rows.Add(values);
            }

            await bulk.WriteToServerAsync(table);
        }


        //public virtual async Task BulkUpdateAsync(IEnumerable<TEntity> entities)
        //{
        //    foreach (var item in entities)
        //    {
        //        Connection.Update(item, transaction: Transaction);
        //    }
        //}

        public virtual async Task BulkUpdateAsync(IEnumerable<TEntity> entities)
        {
            var list = entities.ToList();
            if (!list.Any()) return;

            var tableName = QueriesCreatingHelper.GetTableName<TEntity>();

            // Lấy property key tự động
            var keyProp = typeof(TEntity).GetProperties()
                            .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (keyProp == null)
                throw new Exception($"TEntity {typeof(TEntity).Name} không có property đánh dấu [Key]");

            var props = typeof(TEntity).GetProperties()
                            .Where(p => p.CanRead &&
                                        (p.PropertyType.IsValueType || p.PropertyType == typeof(string) || p.PropertyType == typeof(DateTime?)))
                            .ToList();

            // Tạo DataTable
            var table = new DataTable();
            foreach (var prop in props)
            {
                var colType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                table.Columns.Add(prop.Name, colType);
            }

            foreach (var item in list)
            {
                var values = props.Select(p => p.GetValue(item) ?? DBNull.Value).ToArray();
                table.Rows.Add(values);
            }

            using var bulk = new SqlBulkCopy((SqlConnection)Connection, SqlBulkCopyOptions.Default, (SqlTransaction)Transaction);
            bulk.DestinationTableName = "#TmpUpdate";

            foreach (var prop in props)
                bulk.ColumnMappings.Add(prop.Name, prop.Name);

            // Tạo bảng tạm
            var createTmp = $@"
                CREATE TABLE #TmpUpdate (
                    {string.Join(", ", props.Select(p => $"[{p.Name}] {SqlType(p.PropertyType)}"))}
                )";
                    await Connection.ExecuteAsync(createTmp, transaction: Transaction);

                    await bulk.WriteToServerAsync(table);

                    // MERGE để update
                    var mergeSql = $@"
                MERGE {tableName} AS Target
                USING #TmpUpdate AS Source
                ON Target.{keyProp.Name} = Source.{keyProp.Name}
                WHEN MATCHED THEN 
                    UPDATE SET {string.Join(", ", props.Where(p => p.Name != keyProp.Name)
                                                              .Select(p => $"Target.{p.Name} = Source.{p.Name}"))};
                DROP TABLE #TmpUpdate;
            ";

            await Connection.ExecuteAsync(mergeSql, transaction: Transaction);
        }



        //public virtual async Task BulkDeleteAsync(IEnumerable<TEntity> entities)
        //{
        //    foreach (var item in entities)
        //    {
        //        Connection.Delete(item, transaction: Transaction);
        //    }
        //}

        public virtual async Task BulkDeleteAsync(IEnumerable<TEntity> entities)
        {
            var list = entities.ToList();
            if (!list.Any()) return;

            var tableName = QueriesCreatingHelper.GetTableName<TEntity>();

            var keyProp = typeof(TEntity).GetProperties()
                            .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (keyProp == null)
                throw new Exception($"TEntity {typeof(TEntity).Name} không có property đánh dấu [Key]");

            var table = new DataTable();
            table.Columns.Add(keyProp.Name, Nullable.GetUnderlyingType(keyProp.PropertyType) ?? keyProp.PropertyType);

            foreach (var item in list)
            {
                table.Rows.Add(keyProp.GetValue(item) ?? DBNull.Value);
            }

            using var bulk = new SqlBulkCopy((SqlConnection)Connection, SqlBulkCopyOptions.Default, (SqlTransaction)Transaction);
            bulk.DestinationTableName = "#TmpDelete";
            bulk.ColumnMappings.Add(keyProp.Name, keyProp.Name);

            await Connection.ExecuteAsync($"CREATE TABLE #TmpDelete ({keyProp.Name} {SqlType(keyProp.PropertyType)})", transaction: Transaction);
            await bulk.WriteToServerAsync(table);

            var sql = $@"
                DELETE T
                FROM {tableName} T
                INNER JOIN #TmpDelete D ON T.{keyProp.Name} = D.{keyProp.Name};
                DROP TABLE #TmpDelete;
            ";

            await Connection.ExecuteAsync(sql, transaction: Transaction);
        }

        private string SqlType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            return type switch
            {
                Type t when t == typeof(byte) => "TINYINT",
                Type t when t == typeof(short) => "SMALLINT",
                Type t when t == typeof(int) => "INT",
                Type t when t == typeof(long) => "BIGINT",
                Type t when t == typeof(bool) => "BIT",
                Type t when t == typeof(decimal) => "DECIMAL(18, 2)",
                Type t when t == typeof(double) => "FLOAT",
                Type t when t == typeof(float) => "REAL",
                Type t when t == typeof(DateTime) => "DATETIME",
                Type t when t == typeof(Guid) => "UNIQUEIDENTIFIER",
                Type t when t == typeof(string) => "NVARCHAR(MAX)",
                Type t when t == typeof(byte[]) => "VARBINARY(MAX)",
                Type t when t == typeof(char) => "NCHAR(1)",
                Type t when t == typeof(TimeSpan) => "TIME",
                _ => throw new NotSupportedException($"Type {type.Name} chưa được map sang SQL type.")
            };
        }


    }
}
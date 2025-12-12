using Sample.Common.Database;
using Sample.Common.Domain;
using Sample.Common.Exceptions;
using Dapper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Common.Processing.Providers
{
    public class SequenceProvider : ISequenceProvider
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        const string sequenceQuery = @"SELECT NEXT VALUE FOR {0}";
        const string guidQuery = @"select NEWID()";

        public SequenceProvider(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Guid> GetGuidValue()
        {
            using (var connection = _sqlConnectionFactory.GetConnection(null))
            {
                return await connection.QuerySingleAsync<Guid>(guidQuery);
            }
        }

        public async Task<int> GetIntValue<TEntity>() where TEntity : class, IEntity
        {
            string sequenceName = GetSequenceName<TEntity>();
            using (var connection = _sqlConnectionFactory.GetConnection<TEntity>())
            {
                return await connection.QuerySingleAsync<int>(string.Format(sequenceQuery, sequenceName));
            }
        }

        public async Task<long> GetLongValue<TEntity>() where TEntity : class, IEntity
        {
            string sequenceName = GetSequenceName<TEntity>();
            using (var connection = _sqlConnectionFactory.GetConnection<TEntity>())
            {
                return await connection.QuerySingleAsync<long>(string.Format(sequenceQuery, sequenceName));
            }
        }

        private string GetSequenceName<TEntity>() where TEntity : class, IEntity
        {
            var sequenceAttribute = typeof(TEntity).GetCustomAttributes(typeof(SequenceAttribute), true).FirstOrDefault() as SequenceAttribute;
            if (string.IsNullOrWhiteSpace(sequenceAttribute?.Sequencename))
                throw new DevelopException($"Entity {typeof(TEntity).FullName} not define sequence name!", new { sequenceAttribute });
            return sequenceAttribute.Sequencename;
        }
    }
}

using Sample.Common.Domain;
using System;
using System.Data;

namespace Sample.Common.Database
{
    public interface ISqlConnectionFactory : IDisposable
    {
        IDbConnection GetConnection<TEntity>() where TEntity : class, ISqlModel;

        IDbConnection GetConnection(DatabaseTypes databaseType);

        IDbConnection GetConnection(Type? tEntity);
    }
}
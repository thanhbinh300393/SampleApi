using Sample.Common.Domain;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Sample.Common.Database
{
    public interface IDapperUnitOfWork
    {
        IDbConnection SrpConnection { get; }
        IDbTransaction SrpTransaction { get; }
        IDbConnection DefaultConnection { get; }
        IDbTransaction DefaultTransaction { get; }

        IDbConnection GetConnection< TEntity>() where TEntity : class, ISqlModel;
        IDbConnection GetConnection(DatabaseTypes databaseType);
        IDbConnection GetConnection(Type tEntity);
        IDbTransaction GetTransaction<TEntity>() where TEntity : class, ISqlModel;
        IDbTransaction GetTransaction(DatabaseTypes databaseType);
        IDbTransaction GetTransaction(Type tEntity);

        Task Commit(Guid requestId);
        void Rollback();
        void BeginTransaction(Guid requestId, DatabaseTypes? databaseType = null, IsolationLevel isolation = IsolationLevel.ReadUncommitted);
    }
}

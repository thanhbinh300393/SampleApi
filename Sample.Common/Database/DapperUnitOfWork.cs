using Sample.Common.Domain;
using Sample.Common.Exceptions;
using Sample.Common.Processing;
using Serilog;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Common.Database
{
    public class DapperUnitOfWork : IDapperUnitOfWork
    {
        #region Connection

        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly ICommandsScheduler _commandsScheduler;
        private readonly ILogger _logger;

        private IDbConnection _defaultConnection;
        private IDbTransaction _defaultTransaction;

        public IDbConnection DefaultConnection
        {
            get
            {
                if (_defaultConnection == null || _defaultConnection.State != ConnectionState.Open)
                    _defaultConnection = _sqlConnectionFactory.GetConnection(DatabaseTypes.Default);
                return _defaultConnection;
            }
        }

        public IDbTransaction DefaultTransaction => _defaultTransaction;

        private IDbConnection _srpConnection;
        private IDbTransaction _srpTransaction;

        public IDbConnection SrpConnection
        {
            get
            {
                if (_srpConnection == null || _srpConnection.State != ConnectionState.Open)
                    _srpConnection = _sqlConnectionFactory.GetConnection(DatabaseTypes.Srp);
                return _srpConnection;
            }
        }
        public IDbTransaction SrpTransaction => _srpTransaction;

        private IDbConnection _libConnection;
        private IDbTransaction _libTransaction;

        public IDbConnection LibConnection
        {
            get
            {
                if (_libConnection == null || _libConnection.State != ConnectionState.Open)
                    _libConnection = _sqlConnectionFactory.GetConnection(DatabaseTypes.Lib);
                return _libConnection;
            }
        }
        public IDbTransaction LibTransaction => _libTransaction;

        private IDbConnection _eqeConnection;
        private IDbTransaction _eqeTransaction;
        public IDbConnection EqeConnection
        {
            get
            {
                if (_eqeConnection == null || _eqeConnection.State != ConnectionState.Open)
                    _eqeConnection = _sqlConnectionFactory.GetConnection(DatabaseTypes.Eqe);
                return _eqeConnection;
            }
        }
        public IDbTransaction EqeTransaction => _eqeTransaction;

        private IDbConnection _backupConnection;
        private IDbTransaction _backupTransaction;

        public IDbConnection BackupConnection
        {
            get
            {
                if (_backupConnection == null || _backupConnection.State != ConnectionState.Open)
                    _backupConnection = _sqlConnectionFactory.GetConnection(DatabaseTypes.Backup);
                return _backupConnection;
            }
        }

        public IDbTransaction BackupTransaction => _backupTransaction;

        public IDbConnection GetConnection<TEntity>() where TEntity : class, ISqlModel
        {
            return GetConnection(typeof(TEntity));
        }

        public IDbConnection GetConnection(DatabaseTypes databaseType)
        {
            switch (databaseType)
            {
                case DatabaseTypes.Srp:
                    return SrpConnection;
                case DatabaseTypes.Lib:
                    return LibConnection;
                case DatabaseTypes.Eqe:
                    return EqeConnection;
                case DatabaseTypes.Backup:
                    return BackupConnection;
                default:
                    return DefaultConnection;
                //throw new DevelopException($"DatabaseType {databaseType} not implemented", new { databaseType });
            }
        }

        public IDbConnection GetConnection(Type tEntity)
        {
            var databaseTypeAttribute = tEntity.GetCustomAttributes(typeof(DatabaseTypeAttribute), true).FirstOrDefault() as DatabaseTypeAttribute;
            if (databaseTypeAttribute == null)
                return GetConnection(DatabaseTypes.Default);
            return GetConnection(databaseTypeAttribute.DatabaseType);
        }

        public IDbTransaction GetTransaction<TEntity>() where TEntity : class, ISqlModel
        {
            return GetTransaction(typeof(TEntity));
        }

        public IDbTransaction GetTransaction(DatabaseTypes databaseType)
        {
            switch (databaseType)
            {
                case DatabaseTypes.Srp:
                    return SrpTransaction;
                case DatabaseTypes.Backup:
                    return BackupTransaction;
                case DatabaseTypes.Lib:
                    return LibTransaction;
                case DatabaseTypes.Eqe:
                    return EqeTransaction;
                default:
                    return DefaultTransaction;
                //throw new DevelopException($"DatabaseType {databaseType} not implemented", new { databaseType });
            }
        }

        public IDbTransaction GetTransaction(Type tEntity)
        {
            var databaseTypeAttribute = tEntity.GetCustomAttributes(typeof(DatabaseTypeAttribute), true).FirstOrDefault() as DatabaseTypeAttribute;
            if (databaseTypeAttribute == null)
                return GetTransaction(DatabaseTypes.Default);
            return GetTransaction(databaseTypeAttribute.DatabaseType);
        }

        #endregion

        public DapperUnitOfWork(ISqlConnectionFactory sqlConnectionFactory, ICommandsScheduler commandsScheduler, ILogger logger)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _commandsScheduler = commandsScheduler;
            _logger = logger;
        }

        private Guid? _libRequestId;
        private Guid? _srpRequestId;
        private Guid? _requestId;

        public async Task Commit(Guid requestId)
        {
            if (_requestId == null || requestId == _requestId)
            {
                if (_defaultTransaction != null)
                {
                    _defaultTransaction.Commit();
                    _defaultTransaction = null;
                }

                _defaultConnection?.Close();
                _requestId = null;
            }

            if (_srpRequestId == null || requestId == _srpRequestId)
            {
                if (_srpTransaction != null)
                {
                    _srpTransaction.Commit();
                    _srpTransaction = null;
                }

                _srpConnection?.Close();
                _srpRequestId = null;
            }
        }

        public void Rollback()
        {
            if (_defaultTransaction != null)
                try
                {
                    _defaultTransaction.Rollback();
                }
                catch (Exception)
                {
                }
                finally
                {
                    _defaultTransaction = null;
                }

            if (_srpTransaction != null)
                try
                {
                    _srpTransaction.Rollback();
                }
                catch (Exception)
                {
                }
                finally
                {
                    _srpTransaction = null;
                }

            try
            {
                _defaultConnection?.Close();
            }
            catch (Exception)
            {
            }

            try
            {
                _srpConnection?.Close();
            }
            catch (Exception)
            {
            }
        }


        public void BeginTransaction(Guid requestId, DatabaseTypes? databaseType = null, IsolationLevel isolation = IsolationLevel.ReadUncommitted)
        {
            if (_srpRequestId == null && (databaseType == null || databaseType == DatabaseTypes.Srp))
            {
                _srpRequestId = requestId;
                if ((databaseType == null || databaseType == DatabaseTypes.Srp) && _srpTransaction == null)
                    _srpTransaction = SrpConnection.BeginTransaction(isolation);
                if ((databaseType == null || databaseType == DatabaseTypes.Default) && _defaultTransaction == null)
                    _defaultTransaction = DefaultConnection.BeginTransaction(isolation);
                if ((databaseType == null || databaseType == DatabaseTypes.Backup) && _defaultTransaction == null)
                    _defaultTransaction = DefaultConnection.BeginTransaction(isolation);
            }
            else if (databaseType == DatabaseTypes.Default)
            {
                _requestId = requestId;
                if (_defaultTransaction == null)
                    _defaultTransaction = DefaultConnection.BeginTransaction(isolation);
            }
        }
    }
}
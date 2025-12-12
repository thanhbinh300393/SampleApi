using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sample.Common.Domain;
using Serilog;
using System.Data;

namespace Sample.Common.Database
{
    public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
    {
        private readonly List<IDbConnection> _connections = new();
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public SqlConnectionFactory(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IDbConnection GetConnection<TEntity>() where TEntity : class, ISqlModel
        {
            return GetConnection(typeof(TEntity));
        }

        public IDbConnection GetConnection(DatabaseTypes databaseType)
        {
            var name = databaseType.ToString().ToLower();
            var connectionString = _configuration.GetConnectionString(name);

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException($"Connection string '{name}' not found.");

            var conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                _connections.Add(conn);
                return conn;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Cannot open SQL connection '{name}'");
                conn.Dispose();
                throw;
            }
        }

        public IDbConnection GetConnection(Type? entityType)
        {
            if (entityType == null)
                return GetConnection(DatabaseTypes.Default);
            var attr = entityType.GetCustomAttributes(typeof(DatabaseTypeAttribute), true)
                                 .FirstOrDefault() as DatabaseTypeAttribute;

            return attr == null
                ? GetConnection(DatabaseTypes.Default)
                : GetConnection(attr.DatabaseType);
        }

        public void Dispose()
        {
            foreach (var conn in _connections)
            {
                try
                {
                    conn.Dispose();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error disposing SQL connection");
                }
            }

            _connections.Clear();
        }
    }

}

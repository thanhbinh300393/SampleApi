using Sample.Common.Caching;
using Sample.Common.CQRS.Commands;
using Sample.Common.Database;
using Sample.Common.Domain;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Common.Processing
{
    public class CommandsScheduler : ICommandsScheduler
    {
        private const int TIME_LIMIT_ACCEPT_ENQUEUE = 5;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly ICacheStore _cacheStore;
        private readonly ILogger _logger;
        public CommandsScheduler(ISqlConnectionFactory sqlConnectionFactory, ICacheStore cacheStore, ILogger logger)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _cacheStore = cacheStore;
            _logger = logger;
        }

        public async Task EnqueueAsync(InternalCommandBase command)
        {
            if (_cacheStore.Get(command) != null)
                return;
            else
                _cacheStore.Add(command, command, TimeSpan.FromSeconds(TIME_LIMIT_ACCEPT_ENQUEUE));
        }

        public async Task EnqueueOutBoxAsync(InternalCommandBase command)
        {
            if (_cacheStore.Get(command) != null)
                return;
            else
                _cacheStore.Add(command, command, TimeSpan.FromSeconds(TIME_LIMIT_ACCEPT_ENQUEUE));
        }

        public async Task EnqueueOutBoxToDbAsync()
        {
            
        }
    }
}
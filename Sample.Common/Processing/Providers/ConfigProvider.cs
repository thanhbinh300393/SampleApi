using Sample.Common.Caching;
using Sample.Common.Database;
using Sample.Common.Dependency;
using Sample.Common.Domain;
using Sample.Common.Domain.DBProviders;
using Sample.Common.Extentions;
using Sample.Common.FilterList;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Common.Processing.Providers
{
    public interface IConfigProvider : ISingletonDependency
    {
        Task<T> GetAsync<T>(ConfigKeys key, T defaultValue);
        T Get<T>(ConfigKeys key, T defaultValue);

        Task<List<T>> GetsAsync<T>(ConfigKeys key);
        List<T> Gets<T>(ConfigKeys key);
    }

    public class ConfigProvider : IConfigProvider
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly ISequenceProvider _sequenceProvider;
        private readonly IMemoryCache _memoryCache;

        public ConfigProvider(ISqlConnectionFactory sqlConnectionFactory, ISequenceProvider sequenceProvider, IMemoryCache memoryCache)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _sequenceProvider = sequenceProvider;
            _memoryCache = memoryCache;
        }

        public T Get<T>(ConfigKeys key, T defaultValue)
        {
            return GetAsync(key, defaultValue).Await();
        }

        public async Task<T> GetAsync<T>(ConfigKeys key, T defaultValue)
        {
            var configKey = key.GetName();
            var keyCache = $"{KeyCacheConstants.KEY_TOKEN_LOGGEDOUT}.{configKey}";
            T result;

            if (_memoryCache.TryGetValue(keyCache, out result))
            {
                return result;
            }

            return result;
        }

        public List<T> Gets<T>(ConfigKeys key)
        {
            return GetsAsync<T>(key).Await();
        }

        public async Task<List<T>> GetsAsync<T>(ConfigKeys key)
        {
            var configKey = key.GetName();
            var keyCache = $"{KeyCacheConstants.KEY_TOKEN_LOGGEDOUT}.{configKey}";
            List<T> result = new List<T>(); ;

            if (_memoryCache.TryGetValue(keyCache, out result))
            {
                return result;
            }

            return result;
        }
    }
}

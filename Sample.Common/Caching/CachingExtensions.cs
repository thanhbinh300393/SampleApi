using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Common.Caching
{
    public static class CachingExtensions
    {
        public static async Task<T> GetCacheValueAsync<T>(this IDistributedCache cache, string key) where T : class
        {
            string result = await cache.GetStringAsync(key);
            if (String.IsNullOrEmpty(result))
            {
                return null;
            }
            var deserializedObj = JsonConvert.DeserializeObject<T>(result);
            return deserializedObj;
        }

        public static async Task SetCacheValueAsync<T>(this IDistributedCache cache, string key, T value, CachingTiming cachingTiming) where T : class
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            // Remove item from cache after duration
            var minutes = (int)cachingTiming;
            if (cachingTiming == CachingTiming.Unlimit)
                minutes = int.MaxValue;
            cacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(minutes);

            string result = JsonConvert.SerializeObject(value);

            await cache.SetStringAsync(key, result, cacheEntryOptions);
        }
    }

    public enum CachingTiming
    {
        ShortMinute = 5,
        LongMinute = 30,

        ShortHour = 60,
        LongHour = 240,

        ShortDay = 1440,
        LongDay = 5760,

        Unlimit = 0
    }

}

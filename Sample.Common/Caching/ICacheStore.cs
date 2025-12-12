using System;

namespace Sample.Common.Caching
{
    public interface ICacheStore
    {
        void Add<TItem>(TItem item, ICacheKey<TItem> key, TimeSpan? expirationTime = null);

        void Add<TItem>(TItem item, ICacheKey<TItem> key, DateTime? absoluteExpiration = null);

        TItem Get<TItem>(ICacheKey<TItem> key);

        void Remove<TItem>(ICacheKey<TItem> key);
    }
}
namespace Sample.Common.Caching
{
    public interface ICacheKey<TItem>
    {
        string CacheKey { get; }
    }
}
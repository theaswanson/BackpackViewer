using Microsoft.Extensions.Caching.Memory;

namespace BackpackViewer.Core.Caching;

public class Cache<T>(IMemoryCache memoryCache) : ICache<T> where T : class
{
    public CacheResult<T> Get(string key) =>
        memoryCache.TryGetValue<T>(key, out var result) && result != null
            ? CacheResult<T>.Success(result)
            : CacheResult<T>.NotFound();

    public void Set(string key, T items, TimeSpan expireIn) => memoryCache.Set(key, items, expireIn);
}
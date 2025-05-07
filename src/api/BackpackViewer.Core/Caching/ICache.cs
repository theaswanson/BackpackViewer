namespace BackpackViewer.Core.Caching;

public interface ICache<T> where T : class
{
    CacheResult<T> Get(string key);
    void Set(string key, T items, TimeSpan expireIn);
}
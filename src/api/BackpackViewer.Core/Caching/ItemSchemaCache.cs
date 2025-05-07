using SteamWebAPI2.Models.GameEconomy;

namespace BackpackViewer.Core.Caching;

public class ItemSchemaCache(ICache<SchemaItem[]> cache) : IItemSchemaCache
{
    private const string CacheKey = "item-schema";
    private readonly TimeSpan _expireIn = TimeSpan.FromDays(1);

    public CacheResult<SchemaItem[]> Get() => cache.Get(CacheKey);

    public void Set(SchemaItem[] response) => cache.Set(CacheKey, response, _expireIn);
}
using Steam.Models.GameEconomy;
using SteamWebAPI2.Utilities;

namespace BackpackViewer.Core.Caching;

public class BackpackCache(ICache<ISteamWebResponse<EconItemResultModel>> cache) : IBackpackCache
{
    private readonly TimeSpan _expireIn = TimeSpan.FromMinutes(5);

    public CacheResult<ISteamWebResponse<EconItemResultModel>> Get(ulong steamId) =>
        cache.Get(CacheKey(steamId));

    public void Set(ulong steamId, ISteamWebResponse<EconItemResultModel> response) =>
        cache.Set(CacheKey(steamId), response, _expireIn);

    private static string CacheKey(ulong steamId) => steamId.ToString();
}
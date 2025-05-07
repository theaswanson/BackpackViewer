using Steam.Models.GameEconomy;
using SteamWebAPI2.Utilities;

namespace BackpackViewer.Core.Caching;

public interface IBackpackCache
{
    CacheResult<ISteamWebResponse<EconItemResultModel>> Get(ulong steamId);
    void Set(ulong steamId, ISteamWebResponse<EconItemResultModel> response);
}
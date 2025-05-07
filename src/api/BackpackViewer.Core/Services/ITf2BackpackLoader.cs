using Steam.Models.GameEconomy;
using SteamWebAPI2.Models.GameEconomy;

namespace BackpackViewer.Core.Services
{
    public interface ITf2BackpackLoader
    {
        Task<EconItemResultModel> GetItems(string apiKey, ulong steamId);
        Task<IEnumerable<SchemaItem>> GetItemSchema(string apiKey);
    }
}
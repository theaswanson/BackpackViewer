using Steam.Models.GameEconomy;
using SteamWebAPI2.Models.GameEconomy;

namespace BackpackViewer
{
    public interface IWebApiBackpackLoader
    {
        Task<EconItemResultModel> GetItems(string apiKey, ulong steamId);
        Task<EconItemResultModel> GetMockItems(string filePath);
        Task<IEnumerable<SchemaItem>> GetMockSchema(string filePath);
        Task<IEnumerable<SchemaItem>> GetSchema(string apiKey);
    }
}
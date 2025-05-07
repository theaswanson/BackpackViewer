using Steam.Models.GameEconomy;
using SteamWebAPI2.Models.GameEconomy;

namespace BackpackViewer.Core.Services;

public interface IMockTf2BackpackLoader
{
    Task<EconItemResultModel> GetItems(string filePath);
    Task<IEnumerable<SchemaItem>> GetSchema(string filePath);
}
using BackpackViewer.Core.Models;

namespace BackpackViewer.Core
{
    public interface IItemService
    {
        Task<IEnumerable<ItemSummary>> GetItemsViaSteamCommunityAsync(ulong steamId);
        Task<IEnumerable<ItemSummary>> GetItemsViaMockAsync(string mockResponseFile);
        Task<IEnumerable<ItemSummary>> GetItemsViaWebAPIAsync(ulong steamId, string? playerItemsFile = null, string? itemSchemaFile = null);
    }
}
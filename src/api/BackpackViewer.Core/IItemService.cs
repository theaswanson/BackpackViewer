using BackpackViewer.Core.Models;

namespace BackpackViewer.Core
{
    public interface IItemService
    {
        Task<(IEnumerable<ItemSummary> Items, int BackpackSlots)> GetItemsAsync(ulong steamId, string apiKey, string? playerItemsFile = null, string? itemSchemaFile = null);
    }
}
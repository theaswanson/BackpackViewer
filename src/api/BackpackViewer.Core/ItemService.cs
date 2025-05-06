using BackpackViewer.Core.Models;
using Microsoft.Extensions.Logging;
using Steam.Models.GameEconomy;
using SteamWebAPI2.Models.GameEconomy;

namespace BackpackViewer.Core
{
    public class ItemService : IItemService
    {
        private readonly ITf2BackpackLoader _tf2BackpackLoader;
        private readonly ILogger<ItemService> _logger;

        public ItemService(
            ITf2BackpackLoader tf2BackpackLoader,
            ILogger<ItemService> logger)
        {
            _tf2BackpackLoader = tf2BackpackLoader;
            _logger = logger;
        }

        public async Task<(IEnumerable<ItemSummary> Items, int BackpackSlots)> GetItemsAsync(ulong steamId, string apiKey, string? playerItemsFile = null, string? itemSchemaFile = null)
        {
            var backpackResponse = playerItemsFile != null
                ? await _tf2BackpackLoader.GetMockItems(playerItemsFile)
                : await _tf2BackpackLoader.GetItems(apiKey, steamId);

            var itemSchemaResponse = (itemSchemaFile != null
                ? await _tf2BackpackLoader.GetMockSchema(itemSchemaFile)
                : await _tf2BackpackLoader.GetSchema(apiKey)).ToArray();

            var itemSchemaDictionary = itemSchemaResponse.ToDictionary(item => item.DefIndex, item => item);

            var items = GetItemSummaries(backpackResponse.Items, itemSchemaDictionary);

            return (items, Convert.ToInt32(backpackResponse.NumBackpackSlots));
        }

        private IEnumerable<ItemSummary> GetItemSummaries(
            IReadOnlyCollection<EconItemModel> backpackItems,
            Dictionary<uint, SteamWebAPI2.Models.GameEconomy.SchemaItem> itemSchema,
            bool groupDuplicates = false)
        {
            return GetBackpackItems(backpackItems, groupDuplicates)
                .Select(itemGroup =>
                {
                    itemSchema.TryGetValue(itemGroup.DefIndex, out var schemaItem);

                    return new ItemSummary
                    {
                        Id = itemGroup.DefIndex.ToString(),
                        Quantity = itemGroup.TotalNumberOfItems,
                        Tradable = itemGroup.Tradable,
                        Uses = schemaItem != null && ShouldDisplayQuantity(itemGroup, schemaItem) ? Convert.ToInt32(itemGroup.Quantity) : null,
                        Level = Convert.ToInt32(itemGroup.Level),
                        Name = schemaItem?.ItemName ?? schemaItem?.Name ?? "Unknown item",
                        CustomName = itemGroup.CustomName,
                        Description = schemaItem?.ItemDescription ?? "",
                        CustomDescription = itemGroup.CustomDescription,
                        Type = schemaItem?.ItemTypeName ?? "Unknown type",
                        IconUrl = schemaItem?.ImageUrlLarge ?? string.Empty,
                        BackpackIndex = itemGroup.BackpackIndex,
                        Quality = MapItemQuality(itemGroup.Quality)
                    };
                })
                .ToArray();
        }

        /// <summary>
        /// Based on IEconTool::ShouldDisplayQuantity from the TF2 source code: https://github.com/ValveSoftware/source-sdk-2013/blob/0565403b153dfcde602f6f58d8f4d13483696a13/src/game/shared/econ/econ_item_tools.cpp#L60-L82
        /// </summary>
        private static bool ShouldDisplayQuantity(BackpackItem backpackItem, SchemaItem schemaItem)
        {
            if (schemaItem.Attributes != null && schemaItem.Attributes.Any(a => a.Class == "unlimited_quantity"))
            {
                return false;
            }

            var isTool = schemaItem.ItemClass == "tool";
            var isConsumable = schemaItem.Capabilities.UsableGc == true && backpackItem.Quantity > 0;

            return isTool || isConsumable;
        }

        private static ItemQuality MapItemQuality(uint quality) =>
            Enum.TryParse<ItemQuality>(quality.ToString(), out var itemQuality)
                ? itemQuality
                : ItemQuality.Unknown;

        private IEnumerable<BackpackItem> GetBackpackItems(IReadOnlyCollection<EconItemModel> backpackItems, bool groupDuplicates)
        {
            if (groupDuplicates)
            {
                return GroupedItems(backpackItems);
            }

            return backpackItems.Select(item => new BackpackItem(item));

            IEnumerable<BackpackItem> GroupedItems(IReadOnlyCollection<EconItemModel> items) =>
                items
                    .GroupBy(
                        item => item.DefIndex,
                        item => item,
                        (key, items) =>
                        {
                            var firstItem = items.First();
                            var firstTradable = BackpackItem.ItemIsTradable(firstItem);

                            if (!items.All(item => BackpackItem.ItemIsTradable(item) == firstTradable))
                            {
                                _logger.LogError("Grouped items have differing tradable qualities, {defIndex}",
                                    firstItem.DefIndex);
                            }

                            return new BackpackItem(
                                key,
                                items.Select(i => i.Level).Max(),
                                firstItem.Quantity,
                                items.All(BackpackItem.ItemIsTradable),
                                BackpackItem.GetBackpackIndex(firstItem.Inventory),
                                items.Count(),
                                firstItem.Quality,
                                customName: null,
                                customDescription: null);
                        });
        }
    }
}

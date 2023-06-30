using BackpackViewer.Core.Models;
using Microsoft.Extensions.Logging;
using Steam.Models.GameEconomy;
using System.Text.Json;

namespace BackpackViewer.Core
{
    public class ItemService : IItemService
    {
        private readonly IWebApiBackpackLoader _webAPIBackpackLoader;
        private readonly ISteamCommunityBackpackLoader _steamCommunityBackpackLoader;
        private readonly ILogger<ItemService> _logger;

        public ItemService(
            IWebApiBackpackLoader webAPIBackpackLoader,
            ISteamCommunityBackpackLoader steamCommunityBackpackLoader,
            ILogger<ItemService> logger)
        {
            _webAPIBackpackLoader = webAPIBackpackLoader;
            _steamCommunityBackpackLoader = steamCommunityBackpackLoader;
            _logger = logger;
        }

        public async Task<IEnumerable<ItemSummary>> GetItemsViaWebAPIAsync(ulong steamId, string apiKey, string? playerItemsFile = null, string? itemSchemaFile = null)
        {
            var itemResponse = playerItemsFile == null
                ? await _webAPIBackpackLoader.GetItems(apiKey, steamId)
                : await _webAPIBackpackLoader.GetMockItems(playerItemsFile);

            var schemaResponse = itemSchemaFile == null
                ? await _webAPIBackpackLoader.GetSchema(apiKey)
                : await _webAPIBackpackLoader.GetMockSchema(itemSchemaFile);

            if (!schemaResponse.Any())
            {
                return Enumerable.Empty<ItemSummary>();
            }

            return ParseWebApiResponse(itemResponse, schemaResponse);

            static IEnumerable<ItemSummary> ParseWebApiResponse(EconItemResultModel response, IEnumerable<SteamWebAPI2.Models.GameEconomy.SchemaItem> schemaResponse)
            {
                var groupedItems = response.Items
                    .GroupBy(
                        item => item.DefIndex,
                        item => item,
                        (key, items) => new
                        {
                            DefIndex = key,
                            Quantity = items.Count(),
                            Tradable = items.All(i => !i.FlagCannotTrade.HasValue || !i.FlagCannotTrade.Value)
                        })
                    .OrderBy(i => i.DefIndex);

                return groupedItems.Select(itemGroup =>
                {
                    var schemaItem = schemaResponse.SingleOrDefault(i => i.DefIndex == itemGroup.DefIndex);

                    return new ItemSummary
                    {
                        ClassId = string.Empty,
                        Name = schemaItem?.Name ?? "Unknown item",
                        Type = schemaItem?.ItemTypeName ?? "Unknown type",
                        Quantity = itemGroup.Quantity,
                        IconUrl = schemaItem?.ImageUrlLarge ?? string.Empty,
                        Tradable = itemGroup.Tradable
                    };
                });
            }
        }

        public async Task<IEnumerable<ItemSummary>> GetItemsViaSteamCommunityAsync(ulong steamId)
        {
            _logger.LogInformation("Fetching items for SteamID:{steamId}", steamId);

            var response = await _steamCommunityBackpackLoader.GetItems(steamId);

            EnsureSuccessfulResponse(response);

            _logger.LogInformation("Total inventory count for SteamID:{steamId} is {totalInventoryCount}", steamId, response.TotalInventoryCount);

            return ParseSteamItemsResponse(response);

            static void EnsureSuccessfulResponse(SteamItemsResponse response)
            {
                if (response.Success != 1)
                {
                    throw new Exception("Item response was unsuccessful.");
                }
            }
        }

        public async Task<IEnumerable<ItemSummary>> GetItemsViaMockAsync(string mockResponseFile)
        {
            _logger.LogInformation("Returning mocked response from {mockResponseFile}", mockResponseFile);

            var responseString = await File.ReadAllBytesAsync(mockResponseFile);
            var response = await JsonSerializer.DeserializeAsync<SteamItemsResponse>(
                new MemoryStream(responseString),
                options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return ParseSteamItemsResponse(response);
        }

        private static IEnumerable<ItemSummary> ParseSteamItemsResponse(SteamItemsResponse response)
        {
            var groupedItems = response.Assets
                .GroupBy(
                    item => item.ClassId,
                    item => item,
                    (key, assets) => new
                    {
                        ClassId = key,
                        Quantity = assets.Count()
                    }
                );

            return groupedItems.Select(itemGroup =>
            {
                var description = response.Descriptions.FirstOrDefault(d => d.ClassId == itemGroup.ClassId);

                return new ItemSummary
                {
                    ClassId = itemGroup.ClassId,
                    Name = description?.Name,
                    Type = description?.Type,
                    Quantity = itemGroup.Quantity,
                    IconUrl = $"https://community.cloudflare.steamstatic.com/economy/image/{description?.IconUrl}",
                    Tradable = description?.Tradable == 1
                };
            });
        }
    }
}

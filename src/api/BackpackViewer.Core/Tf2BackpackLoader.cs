using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Steam.Models.GameEconomy;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Models.GameEconomy;
using SteamWebAPI2.Utilities;

namespace BackpackViewer.Core
{
    public enum GetPlayerItemsResult
    {
        Unknown,
        Success,
        InvalidSteamId,
        BackpackIsPrivate,
        SteamIdDoesNotExist,
    }
    
    public class Tf2BackpackLoader : ITf2BackpackLoader
    {
        private readonly ILogger<Tf2BackpackLoader> _logger;

        public Tf2BackpackLoader(ILogger<Tf2BackpackLoader> logger)
        {
            _logger = logger;
        }

        public async Task<EconItemResultModel> GetItems(string apiKey, ulong steamId)
        {
            var api = BuildWebInterface(apiKey);

            var retryCount = 0;

            while (retryCount < 3)
            {
                try
                {
                    var response = await api.GetPlayerItemsAsync(steamId);

                    var status = GetStatus(response.Data.Status);
                    
                    if (status != GetPlayerItemsResult.Success)
                    {
                        _logger.LogError(GetErrorMessage(status));

                        if (status == GetPlayerItemsResult.Unknown)
                        {
                            retryCount++;
                            await Task.Delay(TimeSpan.FromSeconds(3));
                        }
                    }

                    return response.Data;
                }
                catch (Exception ex)
                {
                    retryCount++;

                    _logger.LogError(ex, message: null);

                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
            }

            throw new Exception("Too many API errors when fetching player items.");
        }
        
        private static GetPlayerItemsResult GetStatus(uint status) =>
            status switch
            {
                1 => GetPlayerItemsResult.Success,
                8 => GetPlayerItemsResult.InvalidSteamId,
                15 => GetPlayerItemsResult.BackpackIsPrivate,
                18 => GetPlayerItemsResult.SteamIdDoesNotExist,
                _ => GetPlayerItemsResult.Unknown
            };

        private static string GetErrorMessage(GetPlayerItemsResult status) =>
            status switch
            {
                GetPlayerItemsResult.InvalidSteamId => "Invalid Steam ID.",
                GetPlayerItemsResult.BackpackIsPrivate => "Backpack is private.",
                GetPlayerItemsResult.SteamIdDoesNotExist => "Steam ID does not exist.",
                _ => "Unknown error.",
            };
        
        private class JsonEconItemResultModel
        {
            public uint Status { get; set; }

            public uint NumBackpackSlots { get; set; }

            public IReadOnlyCollection<JsonEconItemModel> Items { get; set; }
        }
        
        private class JsonEconItemModel
        {
            public ulong Id { get; set; }

            public ulong OriginalId { get; set; }

            [JsonPropertyName("defindex")]
            public uint DefIndex { get; set; }

            public uint Level { get; set; }

            public uint Quality { get; set; }

            public ulong Inventory { get; set; }

            public uint Quantity { get; set; }

            public uint Origin { get; set; }

            public IReadOnlyCollection<EconItemEquippedModel> Equipped { get; set; }

            public uint Style { get; set; }

            public IReadOnlyCollection<EconItemAttributeModel> Attributes { get; set; }

            public bool? FlagCannotTrade { get; set; }

            public bool? FlagCannotCraft { get; set; }
        }

        public async Task<EconItemResultModel> GetMockItems(string filePath)
        {
            _logger.LogInformation("Returning mocked items response from {filePath}", filePath);

            var responseString = await File.ReadAllBytesAsync(filePath);

            var parsedFile = await JsonSerializer.DeserializeAsync<JsonEconItemResultModel>(
                new MemoryStream(responseString),
                options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower, PropertyNameCaseInsensitive = true });

            if (parsedFile is null)
            {
                _logger.LogError("Failed to read mocked file.");
                
                return new EconItemResultModel
                {
                    Status = 0,
                    NumBackpackSlots = 0,
                    Items = []
                };
            }
            
            return new EconItemResultModel
            {
                Status = parsedFile.Status,
                NumBackpackSlots = parsedFile.NumBackpackSlots,
                Items = parsedFile.Items.Select(item => new EconItemModel
                {
                    Id = item.Id,
                    OriginalId = item.OriginalId,
                    DefIndex = item.DefIndex,
                    Level = item.Level,
                    Quality = item.Quality,
                    Inventory = item.Inventory,
                    Quantity = item.Quantity,
                    Origin = item.Origin,
                    Equipped = item.Equipped,
                    Style = item.Style,
                    Attributes = item.Attributes,
                    FlagCannotTrade = item.FlagCannotTrade,
                    FlagCannotCraft = item.FlagCannotCraft,
                }).ToArray(),
            };
        }

        public async Task<IEnumerable<SchemaItem>> GetMockSchema(string filePath)
        {
            _logger.LogInformation("Returning mocked schema response from {filePath}", filePath);

            var responseString = await File.ReadAllBytesAsync(filePath);

            return await JsonSerializer.DeserializeAsync<IEnumerable<SchemaItem>>(
                new MemoryStream(responseString),
                options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<SchemaItem>> GetSchema(string apiKey)
        {
            var api = BuildWebInterface(apiKey);

            var items = new List<SchemaItem>(30000);

            uint? start = null;
            var moreItemsToQuery = true;
            var exceptionCount = 0;

            while (moreItemsToQuery)
            {
                try
                {
                    var response = await api.GetSchemaItemsForTF2Async(start: start);

                    if (response.Data.Result.Status != 1)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(3));
                        continue;
                    }

                    items.AddRange(response.Data.Result.Items);

                    if (response.Data.Result.Next is null)
                    {
                        moreItemsToQuery = false;
                    }
                    else
                    {
                        start = response.Data.Result.Next;
                    }
                }
                catch (Exception)
                {
                    exceptionCount++;

                    if (exceptionCount >= 3)
                    {
                        throw new Exception("Too many API failures.");
                    }

                    continue;
                }
            }

            return items;
        }

        private static EconItems BuildWebInterface(string apiKey)
        {
            return new SteamWebInterfaceFactory(apiKey).CreateSteamWebInterface<EconItems>(AppId.TeamFortress2);
        }
    }
}
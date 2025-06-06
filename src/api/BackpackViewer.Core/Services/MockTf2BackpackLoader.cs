﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Steam.Models.GameEconomy;
using SteamWebAPI2.Models.GameEconomy;

namespace BackpackViewer.Core.Services;

public class MockTf2BackpackLoader : IMockTf2BackpackLoader
{
    private readonly ILogger<MockTf2BackpackLoader> _logger;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public MockTf2BackpackLoader(ILogger<MockTf2BackpackLoader> logger)
    {
        _logger = logger;
    }

    public async Task<EconItemResultModel> GetItems(string filePath)
    {
        _logger.LogInformation("Returning mocked items response from {filePath}", filePath);
        
        var responseString = await File.ReadAllBytesAsync(filePath);

        var parsedFile = await Deserialize<JsonEconItemResultModel>(responseString);

        if (parsedFile is null)
        {
            _logger.LogError("Failed to read file.");

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
                Attributes = item.Attributes?.Select(x =>
                    new EconItemAttributeModel
                    {
                        AccountInfo = x.AccountInfo,
                        DefIndex = x.DefIndex,
                        Value = x.Value,
                        FloatValue = x.FloatValue,
                    }).ToArray(),
                FlagCannotTrade = item.FlagCannotTrade,
                FlagCannotCraft = item.FlagCannotCraft,
            }).ToArray(),
        };
    }

    public async Task<IEnumerable<SchemaItem>> GetSchema(string filePath)
    {
        _logger.LogInformation("Returning mocked schema response from {filePath}", filePath);

        var responseString = await File.ReadAllBytesAsync(filePath);

        var parsedFile = await Deserialize<IEnumerable<SchemaItem>>(responseString);

        if (parsedFile is null)
        {
            _logger.LogError("Failed to read file.");

            return [];
        }

        return parsedFile;
    }

    private async Task<T?> Deserialize<T>(byte[] bytes) =>
        await JsonSerializer.DeserializeAsync<T>(new MemoryStream(bytes), _jsonSerializerOptions);

    private class JsonEconItemResultModel
    {
        [JsonPropertyName("status")]
        public uint Status { get; set; }

        [JsonPropertyName("num_backpack_slots")]
        public uint NumBackpackSlots { get; set; }

        [JsonPropertyName("items")]
        public IReadOnlyCollection<JsonEconItemModel> Items { get; set; }
    }

    private class JsonEconItemModel
    {
        public ulong Id { get; set; }

        [JsonPropertyName("original_id")]
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

        public IReadOnlyCollection<JsonEconItemAttributeModel>? Attributes { get; set; }

        
        [JsonPropertyName("flag_cannot_trade")]
        public bool? FlagCannotTrade { get; set; }

        [JsonPropertyName("flag_cannot_craft")]
        public bool? FlagCannotCraft { get; set; }
    }
    
    public class JsonEconItemAttributeModel
    {
        [JsonPropertyName("defindex")]
        public uint DefIndex { get; set; }

        [JsonPropertyName("value")]
        public object Value { get; set; }

        [JsonPropertyName("float_value")]
        public double FloatValue { get; set; }

        [JsonPropertyName("account_info")]
        public EconItemAttributeAccountInfoModel AccountInfo { get; set; }
    }
}
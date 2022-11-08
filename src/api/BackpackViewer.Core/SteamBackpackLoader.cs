using System.Text.Json;
using System.Text.Json.Serialization;

namespace BackpackViewer
{
    public class SteamBackpackLoader
    {
        public async Task<SteamItemsResponse> GetItems(ulong steamId, int appId = 440, string language = "english", int count = 2000)
        {
            var response = await new HttpClient().GetAsync($"https://steamcommunity.com/inventory/{steamId}/{appId}/2?l={language}&count={count}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<SteamItemsResponse>(responseString, options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }

    public class SteamItemsResponse
    {
        public IEnumerable<ItemAsset> Assets { get; set; }
        public IEnumerable<ItemDescription> Descriptions { get; set; }
        [JsonPropertyName("more_items")]
        public int MoreItems { get; set; }
        [JsonPropertyName("last_assetid")]
        public string LastAssetId { get; set; }
        [JsonPropertyName("total_inventory_count")]
        public int TotalInventoryCount { get; set; }
        public int Success { get; set; }
        public int rwgrsn { get; set; }
    }

    public class ItemAsset
    {
        public int AppId { get; set; }
        public string ContextId { get; set; }
        public string AssetId { get; set; }
        public string ClassId { get; set; }
        public string InstanceId { get; set; }
        public string Amount { get; set; }
    }

    public class ItemDescription
    {
        public int AppId { get; set; }
        public string ClassId { get; set; }
        public string InstanceId { get; set; }
        public int Currency { get; set; }
        [JsonPropertyName("background_color")]
        public string BackgroundColor { get; set; }
        [JsonPropertyName("icon_url")]
        public string IconUrl { get; set; }
        [JsonPropertyName("icon_url_large")]
        public string IconUrlLarge { get; set; }
        public IEnumerable<ItemSubDescription> Descriptions { get; set; }
        public int Tradable { get; set; }
        public IEnumerable<ItemAction> Actions { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("name_color")]
        public string NameColor { get; set; }
        public string Type { get; set; }
        [JsonPropertyName("market_name")]
        public string MarketName { get; set; }
        [JsonPropertyName("market_hash_name")]
        public string MarketHashName { get; set; }
        public int Commodity { get; set; }
        [JsonPropertyName("market_tradable_restriction")]
        public int MarketTradableRestriction { get; set; }
        [JsonPropertyName("market_marketable_restriction")]
        public int MarketMarketableRestriction { get; set; }
        public int Marketable { get; set; }
        public IEnumerable<ItemTag> Tags { get; set; }
    }

    public class ItemSubDescription
    {
        public string Value { get; set; }
        public string Color { get; set; }
    }

    public class ItemAction
    {
        public string Link { get; set; }
        public string Name { get; set; }
    }

    public class ItemTag
    {
        public string Category { get; set; }
        [JsonPropertyName("internal_name")]
        public string InternalName { get; set; }
        [JsonPropertyName("localized_category_name")]
        public string LocalizedCategoryName { get; set; }
        [JsonPropertyName("localized_tag_name")]
        public string LocalizedTagName { get; set; }
    }
}
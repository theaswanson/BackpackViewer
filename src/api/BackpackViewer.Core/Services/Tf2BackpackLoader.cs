using BackpackViewer.Core.Caching;
using BackpackViewer.Core.Models;
using Microsoft.Extensions.Logging;
using Steam.Models.GameEconomy;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Models.GameEconomy;
using SteamWebAPI2.Utilities;

namespace BackpackViewer.Core.Services;

public class Tf2BackpackLoader : ITf2BackpackLoader
{
    private readonly IBackpackCache _backpackCache;
    private readonly IItemSchemaCache _itemSchemaCache;
    private readonly ILogger<Tf2BackpackLoader> _logger;

    public Tf2BackpackLoader(IBackpackCache backpackCache, IItemSchemaCache itemSchemaCache, ILogger<Tf2BackpackLoader> logger)
    {
        _backpackCache = backpackCache;
        _itemSchemaCache = itemSchemaCache;
        _logger = logger;
    }

    public async Task<EconItemResultModel> GetItems(string apiKey, ulong steamId)
    {
        var cacheResult = _backpackCache.Get(steamId);

        if (cacheResult.Found)
        {
            _logger.LogInformation("Returning cached backpack for Steam ID {steamId}.", steamId);
            return cacheResult.Value.Data;
        }
        
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
                        continue;
                    }
                }
                
                _logger.LogInformation("Caching backpack for Steam ID {steamId}.", steamId);
                _backpackCache.Set(steamId, response);

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
        
    public async Task<IEnumerable<SchemaItem>> GetItemSchema(string apiKey)
    {
        var cacheResult = _itemSchemaCache.Get();

        if (cacheResult.Found)
        {
            _logger.LogInformation("Returning cached item schema.");
            return cacheResult.Value;
        }
        
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
        
        _logger.LogInformation("Caching item schema.");
        _itemSchemaCache.Set(items.ToArray());

        return items;
    }

    private static EconItems BuildWebInterface(string apiKey)
    {
        return new SteamWebInterfaceFactory(apiKey).CreateSteamWebInterface<EconItems>(AppId.TeamFortress2);
    }
}
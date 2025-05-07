using SteamWebAPI2.Models.GameEconomy;

namespace BackpackViewer.Core.Caching;

public interface IItemSchemaCache
{
    CacheResult<SchemaItem[]> Get();
    void Set(SchemaItem[] response);
}
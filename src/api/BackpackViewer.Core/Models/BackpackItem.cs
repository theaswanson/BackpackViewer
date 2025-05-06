using Steam.Models.GameEconomy;

namespace BackpackViewer.Core.Models;

public class BackpackItem
{
    public uint DefIndex { get; }
    public uint Level { get; }
    public uint Quantity { get; }
    public bool Tradable { get; }
    public int BackpackIndex { get; }
    public int TotalNumberOfItems { get; }
    public uint Quality { get; }
    public string? CustomName { get; }
    public string? CustomDescription { get; }

    public BackpackItem(uint defIndex, uint level, uint quantity, bool tradable, int backpackIndex,
        int totalNumberOfItems, uint quality, string? customName = null, string? customDescription = null)
    {
        DefIndex = defIndex;
        Level = level;
        Quantity = quantity;
        Tradable = tradable;
        BackpackIndex = backpackIndex;
        TotalNumberOfItems = totalNumberOfItems;
        Quality = quality;
        CustomName = customName;
        CustomDescription = customDescription;
    }

    public BackpackItem(EconItemModel item) : this(item.DefIndex, item.Level, item.Quantity, ItemIsTradable(item),
        GetBackpackIndex(item.Inventory), 1, item.Quality, GetCustomName(item), GetCustomDescription(item))
    {
    }

    public static bool ItemIsTradable(EconItemModel i)
    {
        // TODO: fix this method. Every item is being marked as tradable for some reason.
        
        if (!i.FlagCannotTrade.HasValue)
        {
            return true;
        }

        return !i.FlagCannotTrade.Value;
    }

    public static int GetBackpackIndex(ulong inventoryToken)
    {
        // https://wiki.teamfortress.com/wiki/WebAPI/GetPlayerItems#Inventory_token
        // Inventory is defined as 64-bit, but it should be 32-bit. Discard the last 32-bits.
        uint inventoryToken32 = (uint)(inventoryToken & 0xFFFFFFFF);
        // Get the backpack slot info from the first 16-bits
        ushort backpackSlot16 = (ushort)(inventoryToken32 & 0xFFFF);
        // Get the class flags from the last 16-bits.
        ushort classFlags16 = (ushort)((inventoryToken32 >> 16) & 0xFFFF);

        var backpackIndex = Convert.ToInt32(backpackSlot16);

        return backpackIndex;
    }

    public static string? GetCustomName(EconItemModel item) => GetAttributeValue(item, 500);

    public static string? GetCustomDescription(EconItemModel item) => GetAttributeValue(item, 501);

    public static string? GetAttributeValue(EconItemModel item, uint defIndex)
    {
        if (item.Attributes is null || item.Attributes.Count == 0)
        {
            return null;
        }

        var attribute = item.Attributes.SingleOrDefault(a => a.DefIndex == defIndex);

        return attribute?.Value.ToString();
    }
}
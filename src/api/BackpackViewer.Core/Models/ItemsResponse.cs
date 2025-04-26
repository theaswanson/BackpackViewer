namespace BackpackViewer.Core.Models;

public class ItemsResponse
{
    public IEnumerable<ItemSummary> Items { get; set; } = new List<ItemSummary>();
    public int TotalBackpackSlots { get; set; }
}
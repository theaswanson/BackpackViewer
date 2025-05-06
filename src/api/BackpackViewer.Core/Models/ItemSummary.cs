namespace BackpackViewer.Core.Models
{
    public class ItemSummary
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? CustomName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string? CustomDescription { get; set; }
        /// <summary>
        /// When grouping items, represents the number of duplicates in the user's backpack.
        /// When not grouping items, this will always be 1.
        /// </summary>
        public int Quantity { get; set; }
        public string IconUrl { get; set; }
        public bool Tradable { get; set; }
        public int? Level { get; set; }
        /// <summary>
        /// The remaining number of times this item can be used before it is consumed.
        /// Null if the item cannot be consumed (has infinite uses).
        /// </summary>
        public int? Uses { get; set; }
        public int BackpackIndex { get; set; }
        public ItemQuality Quality { get; set; }
    }
}
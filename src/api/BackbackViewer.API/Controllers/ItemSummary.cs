namespace BackbackViewer.API.Controllers
{
    public class ItemSummary
    {
        public string ClassId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public string IconUrl { get; set; }
        public bool? Tradable { get; set; }
    }
}
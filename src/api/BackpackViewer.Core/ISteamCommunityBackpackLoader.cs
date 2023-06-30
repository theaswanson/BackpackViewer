namespace BackpackViewer
{
    public interface ISteamCommunityBackpackLoader
    {
        Task<SteamItemsResponse> GetItems(ulong steamId, int appId = 440, string language = "english", int count = 2000);
    }
}
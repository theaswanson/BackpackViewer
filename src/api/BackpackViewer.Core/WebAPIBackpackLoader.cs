using Steam.Models.GameEconomy;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

namespace BackpackViewer
{
    public class WebAPIBackpackLoader
    {
        public async Task<EconItemResultModel> GetItems(string apiKey, ulong steamId)
        {
            var api = BuildWebInterface(apiKey);

            var response = await api.GetPlayerItemsAsync(steamId);

            return response.Data;
        }

        private static EconItems BuildWebInterface(string apiKey)
        {
            return new SteamWebInterfaceFactory(apiKey).CreateSteamWebInterface<EconItems>(AppId.TeamFortress2);
        }
    }
}
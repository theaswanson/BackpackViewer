namespace BackpackViewer.Core.Models;

public enum GetPlayerItemsResult
{
    Unknown,
    Success,
    InvalidSteamId,
    BackpackIsPrivate,
    SteamIdDoesNotExist,
}
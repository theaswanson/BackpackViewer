using BackpackViewer.Core.Models;
using BackpackViewer.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackpackViewer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly IItemService _itemService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ItemsController> _logger;

    public ItemsController(
        IWebHostEnvironment env,
        IItemService itemService,
        IConfiguration configuration,
        ILogger<ItemsController> logger)
    {
        _env = env;
        _itemService = itemService;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet]
    [Route("{steamId}")]
    public async Task<ItemsResponse> Get(ulong steamId, [FromQuery] bool useMockResponse)
    {
        if (steamId <= 0) throw new ArgumentOutOfRangeException(nameof(steamId));

        var (items, backpackSlots) = await _itemService.GetItemsAsync(
            steamId,
            _configuration.GetRequiredSection("Steam").GetValue<string>("ApiKey"),
            useMockResponse ? Path.Combine(_env.WebRootPath, "player-items.json") : null,
            useMockResponse ? Path.Combine(_env.WebRootPath, "full-schema.json") : null);

        return new ItemsResponse
        {
            Items = items,
            TotalBackpackSlots = backpackSlots
        };
    }
}
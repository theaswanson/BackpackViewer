using BackpackViewer;
using BackpackViewer.Core;
using BackpackViewer.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BackbackViewer.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IItemService _itemService;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(
            IWebHostEnvironment env,
            IItemService itemService,
            ILogger<ItemsController> logger)
        {
            _env = env;
            _itemService = itemService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemSummary>> Get(bool useMockResponse)
        {
            const ulong SteamId = 76561198084230569;

            return await _itemService.GetItemsViaWebAPIAsync(
                SteamId,
                useMockResponse ? Path.Combine(_env.WebRootPath, "player-items.json") : null,
                Path.Combine(_env.WebRootPath, "full-schema.json"));
        }
    }
}
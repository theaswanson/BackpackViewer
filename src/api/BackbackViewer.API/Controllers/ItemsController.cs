using BackpackViewer;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BackbackViewer.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly SteamBackpackLoader _backpackLoader;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(
            IWebHostEnvironment env,
            SteamBackpackLoader backpackLoader,
            ILogger<ItemsController> logger)
        {
            _env = env;
            _backpackLoader = backpackLoader;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemSummary>> Get(bool useMockResponse)
        {
            var response = useMockResponse
                ? await MockItemResponse()
                : await RealItemResponse();

            if (response.Success != 1)
            {
                throw new Exception("Failed to fetch items.");
            }

            var groupedItems = response.Assets
                .GroupBy(
                    item => item.ClassId,
                    item => item,
                    (key, assets) => new
                    {
                        ClassId = key,
                        Quantity = assets.Count()
                    }
                );

            return groupedItems.Select(itemGroup =>
            {
                var description = response.Descriptions.FirstOrDefault(d => d.ClassId == itemGroup.ClassId);

                return new ItemSummary
                {
                    ClassId = itemGroup.ClassId,
                    Name = description?.Name,
                    Type = description?.Type,
                    Quantity = itemGroup.Quantity,
                    IconUrl = $"https://community.cloudflare.steamstatic.com/economy/image/{description?.IconUrl}",
                    Tradable = description?.Tradable == 1
                };
            });

            async Task<SteamItemsResponse> MockItemResponse()
            {
                var mockResponseFile = Path.Combine(_env.WebRootPath, "sample-response.json");

                _logger.LogInformation($"Returning mocked response from {mockResponseFile}");

                var responseString = await System.IO.File.ReadAllBytesAsync(mockResponseFile);

                return await JsonSerializer.DeserializeAsync<SteamItemsResponse>(new MemoryStream(responseString), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            async Task<SteamItemsResponse> RealItemResponse()
            {
                _logger.LogInformation("Returning real response.");

                return await _backpackLoader.GetItems(76561198084230569);
            }
        }
    }
}
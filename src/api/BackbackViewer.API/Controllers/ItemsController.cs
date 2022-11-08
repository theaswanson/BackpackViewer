using BackpackViewer;
using Microsoft.AspNetCore.Mvc;

namespace BackbackViewer.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly SteamBackpackLoader _backpackLoader;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(SteamBackpackLoader backpackLoader, ILogger<ItemsController> logger)
        {
            _backpackLoader = backpackLoader;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemSummary>> Get()
        {
            var useMockResponse = true;

            return useMockResponse
                ? MockItemResponse()
                : await RealItemResponse();

            IEnumerable<ItemSummary> MockItemResponse()
            {
                _logger.LogInformation("Returning mocked response.");

                return new List<ItemSummary>
                {
                    new ItemSummary
                    {
                        ClassId = "5564",
                        Name = "Reclaimed Metal",
                        Type = "Level 2 Craft Item",
                        Quantity = 5,
                        IconUrl = $"https://community.cloudflare.steamstatic.com/economy/image/fWFc82js0fmoRAP-qOIPu5THSWqfSmTELLqcUywGkijVjZULUrsm1j-9xgEbZQsUYhTkhzJWhsO0Mv6NGucF1YJlscMEgDdvxVYsMLPkMmFjI1OSUvMHDPBp9lu0CnVluZQxA9Gwp-hIOVK4sMMNWF4"
                    },
                    new ItemSummary
                    {
                        ClassId = "2674",
                        Name = "Refined Metal",
                        Type = "Level 3 Craft Item",
                        Quantity = 27,
                        IconUrl = $"https://community.cloudflare.steamstatic.com/economy/image/fWFc82js0fmoRAP-qOIPu5THSWqfSmTELLqcUywGkijVjZULUrsm1j-9xgEbZQsUYhTkhzJWhsO1Mv6NGucF1Ygzt8ZQijJukFMiMrbhYDEwI1yRVKNfD6xorQ3qW3Jr6546DNPuou9IOVK4p4kWJaA"
                    }
                };
            }

            async Task<IEnumerable<ItemSummary>> RealItemResponse()
            {
                _logger.LogInformation("Returning real response.");

                var itemsResponse = await _backpackLoader.GetItems(76561198084230569);

                if (itemsResponse.Success != 1)
                {
                    throw new Exception("Failed to fetch items.");
                }

                return itemsResponse.Assets.Select(a =>
                {
                    var descriptions = itemsResponse.Descriptions.Where(d => d.ClassId == a.ClassId);
                    var description = descriptions.FirstOrDefault();

                    return new ItemSummary
                    {
                        ClassId = a.ClassId,
                        Name = description?.Name,
                        Type = description?.Type,
                        Quantity = descriptions.Any() ? descriptions.Count() : 1,
                        IconUrl = $"https://community.cloudflare.steamstatic.com/economy/image/{description?.IconUrl}"
                    };
                });
            }
        }
    }
}
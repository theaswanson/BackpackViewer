using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text.Json;
using BackpackViewer.Core;
using BackpackViewer.Core.Caching;
using BackpackViewer.Core.Services;
using Steam.Models.GameEconomy;

namespace BackpackViewer.Tests
{
    [TestFixture]
    public class Tf2BackpackLoaderTests
    {
        private Tf2BackpackLoader _backpackLoader;
        
        private ILogger<Tf2BackpackLoader> _logger;
        private IBackpackCache _backpackCache;
        private IItemSchemaCache _itemSchemaCache;

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger<Tf2BackpackLoader>>();
            _backpackCache = Substitute.For<IBackpackCache>();
            _itemSchemaCache = Substitute.For<IItemSchemaCache>();

            _backpackLoader = new Tf2BackpackLoader(_backpackCache, _itemSchemaCache, _logger);
        }

        [Test]
        public async Task GivenValidApiKeyAndSteamId_GetsPlayerItems()
        {
            var items = await GetPlayerItemsAsync();

            Assert.Multiple(() =>
            {
                Assert.That(items, Is.Not.Null);
                Assert.That(items.Status, Is.EqualTo(1));
                Assert.That(items.Items, Is.Not.Empty);
                Assert.That(items.NumBackpackSlots, Is.GreaterThanOrEqualTo(1));
            });
        }

        private async Task<EconItemResultModel> GetPlayerItemsAsync() =>
            await _backpackLoader.GetItems(TestConstants.ApiKey, TestConstants.SteamId);

        [Test]
        public async Task GivenValidApiKey_GetsItemSchema()
        {
            var schemaItems = await _backpackLoader.GetItemSchema(TestConstants.ApiKey);

            Assert.That(schemaItems, Is.Not.Empty);
        }

        [Test]
        [Explicit]
        public async Task DownloadPlayerItemsJson()
        {
            var items = await GetPlayerItemsAsync();

            if (items is null || items.Status != 1)
            {
                Assert.Fail("Invalid response.");
            }

            await WriteToFileAsync(items, "player-items.json");
        }

        [Test]
        [Explicit]
        public async Task DownloadItemSchemaJson()
        {
            var schemaItems = await _backpackLoader.GetItemSchema(TestConstants.ApiKey);

            Assert.That(schemaItems, Is.Not.Empty);

            await WriteToFileAsync(schemaItems, "full-schema.json");
        }

        private static async Task WriteToFileAsync(object data, string fileName)
        {
            Console.WriteLine("Saving contents to {0}...", Path.GetFullPath(fileName));
            
            var memoryStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(memoryStream, data);
            
            var fileStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);

            memoryStream.WriteTo(fileStream);
        }
    }
}
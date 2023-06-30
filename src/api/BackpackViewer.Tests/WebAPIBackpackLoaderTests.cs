using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text.Json;

namespace BackpackViewer.Tests
{
    [TestFixture]
    public class WebApiBackpackLoaderTests
    {
        private WebApiBackpackLoader _backpackLoader;
        private ILogger<WebApiBackpackLoader> _logger;

        private const string _apiKey = "API-KEY-HERE";
        private const ulong _steamId = 0u;

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger<WebApiBackpackLoader>>();

            _backpackLoader = new WebApiBackpackLoader(_logger);
        }

        [Test]
        public async Task GetPlayerItems()
        {
            var items = await _backpackLoader.GetItems(_apiKey, _steamId);

            Assert.Multiple(() =>
            {
                Assert.That(items, Is.Not.Null);
                Assert.That(items.Status, Is.EqualTo(1));
                Assert.That(items.Items, Is.Not.Empty);
                Assert.That(items.NumBackpackSlots, Is.GreaterThanOrEqualTo(1));
            });
        }

        [Test]
        public async Task GetSchema()
        {
            var schemaItems = await _backpackLoader.GetSchema(_apiKey);

            var memoryStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(memoryStream, schemaItems);

            var fileStream = new FileStream("full-schema.json", FileMode.CreateNew, FileAccess.Write);

            memoryStream.WriteTo(fileStream);
        }
    }
}
namespace BackpackViewer.Tests
{
    public class WebAPIBackpackLoaderTests
    {
        private WebAPIBackpackLoader _backpackLoader;

        [SetUp]
        public void Setup()
        {
            _backpackLoader = new WebAPIBackpackLoader();
        }

        [Test]
        public async Task Test1()
        {
            // TODO: set API key
            var apiKey = "YOUR-API-KEY-HERE";
            // TODO: set Steam ID
            var steamId = 0u;
            var items = await _backpackLoader.GetItems(apiKey, steamId);

            Assert.Multiple(() =>
            {
                Assert.That(items, Is.Not.Null);
                Assert.That(items.Status, Is.EqualTo(1));
                Assert.That(items.Items, Is.Not.Empty);
                Assert.That(items.NumBackpackSlots, Is.GreaterThanOrEqualTo(1));
            });
        }
    }
}
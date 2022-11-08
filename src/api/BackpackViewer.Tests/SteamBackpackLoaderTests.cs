namespace BackpackViewer.Tests
{
    public class SteamBackpackLoaderTests
    {
        private SteamBackpackLoader _backpackLoader;

        [SetUp]
        public void Setup()
        {
            _backpackLoader = new SteamBackpackLoader();
        }

        [TestCase()]
        public async Task GetSuccessfulResponse()
        {
            // TODO: set Steam ID
            var steamId = 0u;
            var items = await _backpackLoader.GetItems(steamId);

            Assert.That(items, Is.Not.Null);
            Assert.That(items.Success, Is.EqualTo(1));
        }
    }
}
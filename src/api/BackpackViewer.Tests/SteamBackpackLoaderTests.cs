namespace BackpackViewer.Tests
{
    public class SteamBackpackLoaderTests
    {
        private SteamCommunityBackpackLoader _backpackLoader;

        [SetUp]
        public void Setup()
        {
            _backpackLoader = new SteamCommunityBackpackLoader();
        }

        [Test]
        public async Task GetSuccessfulResponse()
        {
            var items = await _backpackLoader.GetItems(TestConstants.SteamId);

            Assert.That(items, Is.Not.Null);
            Assert.That(items.Success, Is.EqualTo(1));
        }
    }
}
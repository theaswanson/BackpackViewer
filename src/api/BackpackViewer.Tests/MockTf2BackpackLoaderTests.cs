using System.Text.Json;
using BackpackViewer.Core.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Steam.Models.GameEconomy;

namespace BackpackViewer.Tests;

[TestFixture]
public class MockTf2BackpackLoaderTests
{
    private MockTf2BackpackLoader _mockTf2BackpackLoader;
    private ILogger<MockTf2BackpackLoader> _logger;
    
    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<MockTf2BackpackLoader>>();
        _mockTf2BackpackLoader = new MockTf2BackpackLoader(_logger);
    }

    [Test]
    public async Task CanParseMockFile()
    {
        var result =
            await _mockTf2BackpackLoader.GetItems(Path.Combine("../../../../BackpackViewer.API/www/player-items.json"));

        result.Should().NotBeNull();
        result.Status.Should().Be(1);
        result.NumBackpackSlots.Should().Be(1500);
        result.Items.Should().NotBeEmpty();

        result.Items.First().Should().BeEquivalentTo(new EconItemModel
        {
            Id = 1488291227,
            OriginalId = 1488291227,
            DefIndex = 166,
            Level = 5,
            Quality = 6,
            Inventory = 2147484160,
            Quantity = 1,
            Origin = 0,
            FlagCannotCraft = null,
            FlagCannotTrade = true,
            Attributes =
            new List<EconItemAttributeModel>()
            {
                new() { DefIndex = 143, Value = JsonElementFromObject(1361224728), FloatValue = 43658608640 },
                new() { DefIndex = 746, Value = JsonElementFromObject(1065353216), FloatValue = 1 },
                new() { DefIndex = 292, Value = JsonElementFromObject(1115684864), FloatValue = 64 },
                new() { DefIndex = 388, Value = JsonElementFromObject(1115684864), FloatValue = 64 },
                new() { DefIndex = 153, Value = JsonElementFromObject(1065353216), FloatValue = 1 },
            }
        }, opt => opt.ComparingByMembers<JsonElement>());
    }
    
    /// <summary>
    /// https://stackoverflow.com/a/67003925
    /// </summary>
    private static JsonElement JsonElementFromObject(object value)
    {
        var jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(value);
        using var doc = JsonDocument.Parse(jsonUtf8Bytes);
        return doc.RootElement.Clone();
    }
}
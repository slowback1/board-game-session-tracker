using Database.Common.DTOs;
using Database.Common.Game;
using Database.Common.Test.Storers;

namespace Database.Common.Test.Game;

public class PlayerInventoryRepositoryTests : BaseDbTest
{
    private PlayerInventoryRepository _repository { get; set; }

    [SetUp]
    public void SetUpRepository()
    {
        _repository = new PlayerInventoryRepository(DataStorer);
    }

    [Test]
    public async Task ReturnsAValidApiResponseWhenGettingPlayerInventoryForTheGame()
    {
        var result = await _repository.GetPlayerInventoryForGame("1234");

        Assert.That(result, Is.Not.Null);

        Assert.That(result.Response!.Count, Is.GreaterThan(0));
        Assert.That(result.Response.First().PlayerName, Is.EqualTo("bob"));
    }

    [Test]
    public async Task ReturnsAnErrorMessageWhenGetPlayerInventoryForGameIsNull()
    {
        var result = await _repository.GetPlayerInventoryForGame("null");

        Assert.That(result.Response, Is.Null);
        Assert.That(result.Errors, Is.Not.Null);

        var errors = result.Errors!;

        Assert.That(errors, Contains.Item("Game not found."));
    }

    [Test]
    public async Task ReturnsTheUpdatedInventoryItemWhenUpdatingASingleInventoryItem()
    {
        var result = await _repository.UpdatePlayerInventoryItem("game", new UpdatePlayerInventoryRequest
        {
            Amount = 1,
            PlayerId = "1",
            InventoryTypeOptionId = "1"
        });

        Assert.That(result.Response, Is.Not.Null);
        Assert.That(result.Response, Is.EqualTo(DataStorer.LastUpdateInventoryItem));
    }

    [Test]
    public async Task StoresTheUpdatedInventoryInTheDataStorer()
    {
        var result = await _repository.UpdatePlayerInventoryItem("game", new UpdatePlayerInventoryRequest
        {
            Amount = 1,
            PlayerId = "1",
            InventoryTypeOptionId = "1"
        });

        Assert.That(DataStorer.LastUpdateInventoryItem, Is.Not.Null);
    }

    [Test]
    public async Task ReturnsTheUpdatedPlayerInventoriesWhenUpdatingAllPlayerInventories()
    {
        var result = await _repository.UpdateAllPlayersInventories("1234", new UpdateAllPlayerInventoriesRequest
        {
            InventoryTypeOptionId = "1",
            AmountChanged = 5
        });

        Assert.That(result.Response, Is.Not.Null);
        Assert.That(result.Response.Count, Is.GreaterThan(0));
    }

    [Test]
    public async Task AddsTheAmountToCurrentInventoryWhenUpdatingAllPlayersInventory()
    {
        await _repository.UpdatePlayerInventoryItem("game", new UpdatePlayerInventoryRequest
        {
            Amount = 1,
            PlayerId = "1",
            InventoryTypeOptionId = "1"
        });

        var result = await _repository.UpdateAllPlayersInventories("game", new UpdateAllPlayerInventoriesRequest
        {
            InventoryTypeOptionId = "1",
            AmountChanged = 5
        });

        Assert.That(result.Response.First().Amount, Is.EqualTo(6));
    }

    [Test]
    public async Task ReturnsAnErrorResultIfGameIsNotFound()
    {
        var result = await _repository.UpdateAllPlayersInventories("null",
            new UpdateAllPlayerInventoriesRequest { InventoryTypeOptionId = "1234", AmountChanged = 1 });

        Assert.That(result.Errors, Is.Not.Null);

        Assert.That(result.Errors!.First(), Is.EqualTo("Game not found."));
    }
}
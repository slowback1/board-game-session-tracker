using Database.Common.DTOs;
using Database.MySql.Models;
using Microsoft.EntityFrameworkCore;
using InventoryTypeOption = Database.Common.DTOs.InventoryTypeOption;

namespace Database.MySql.Test;

public class MySqlDataStorer_PlayerInventory_Tests : BaseDbTest
{
    private UserDTO _user { get; set; }
    private GameDTO _game { get; set; }
    private InventoryTypeResponse _inventoryType { get; set; }

    [SetUp]
    public async Task SetUpTestData()
    {
        _user = await _repository.CreateUser("user", "pass");
        _game = await _repository.CreateGame("game", _user.UserId);
        _inventoryType = await _repository.CreateInventoryType(_game.GameId, new CreateInventoryTypeDTO
        {
            Name = "spells",
            Options = new List<InventoryTypeOption>
            {
                new()
                {
                    Label = "label",
                    Value = "value"
                }
            }
        });
    }

    [Test]
    public async Task CanGetInventoryForTheGame()
    {
        var inventory = await _repository.GetInventoryForGame(_game.GameId);

        Assert.That(inventory, Is.Not.Null);
        Assert.That(inventory.Count(), Is.EqualTo(1));

        var firstPlayer = inventory.First();

        Assert.That(firstPlayer.PlayerId, Is.EqualTo(_user.UserId));
        Assert.That(firstPlayer.PlayerName, Is.EqualTo("user"));
        Assert.That(firstPlayer.Inventory.Count(), Is.EqualTo(1));

        var firstInventory = firstPlayer.Inventory.First();

        Assert.That(firstInventory.InventoryTypeName, Is.EqualTo("spells"));
        Assert.That(firstInventory.InventoryTypeId, Is.EqualTo(_inventoryType.InventoryTypeId));
    }

    [Test]
    public async Task GettingInventoryZeroesOutAnyOptionsThatArentStoredInTheDB()
    {
        var inventory = await _repository.GetInventoryForGame(_game.GameId);
        var firstPlayer = inventory.First();
        var firstInventory = firstPlayer.Inventory.First();

        Assert.That(firstInventory.Items.Count(), Is.EqualTo(1));

        var firstItem = firstInventory.Items.First();

        Assert.That(firstItem.Name, Is.EqualTo(_inventoryType.Options.First().Label));
        Assert.That(firstItem.InventoryTypeOptionId, Is.Not.Null);
        Assert.That(firstItem.Amount, Is.EqualTo(0));
    }

    [Test]
    public async Task GettingInventoryGetsTheStoredValue()
    {
        await CreateTestPlayerInventory();

        var inventory = await _repository.GetInventoryForGame(_game.GameId);
        var firstPlayer = inventory.First();
        var firstInventory = firstPlayer.Inventory.First();

        Assert.That(firstInventory.Items.Count(), Is.EqualTo(1));

        var firstItem = firstInventory.Items.First();

        Assert.That(firstItem.Amount, Is.EqualTo(5));
    }

    [Test]
    public async Task CanUpdatePlayerInventory()
    {
        var request = MakeUpdateRequest(5);

        var result = await _repository.UpdateInventoryForPlayer(_game.GameId, request);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Amount, Is.EqualTo(5));
        Assert.That(result.InventoryTypeOptionId, Is.EqualTo(request.InventoryTypeOptionId));
        Assert.That(result.Name, Is.EqualTo(_context.InventoryTypeOptions.First().Label));
    }

    [Test]
    public async Task UpdatingPlayerInventoryStoresTheUpdatesInTheDB()
    {
        var request = MakeUpdateRequest(5);

        var result = await _repository.UpdateInventoryForPlayer(_game.GameId, request);

        var stored =
            await _context.PlayerItems.FirstAsync(i =>
                i.InventoryTypeOptionId == Guid.Parse(request.InventoryTypeOptionId));

        Assert.That(stored.Amount, Is.EqualTo(5));
    }

    [Test]
    public async Task UpdatingPlayerInventoryTypeTwiceJustUpdatesTheDBAndDoesntInsert()
    {
        var request = MakeUpdateRequest(5);

        var result = await _repository.UpdateInventoryForPlayer(_game.GameId, request);

        var request2 = MakeUpdateRequest(13);
        var result2 = await _repository.UpdateInventoryForPlayer(_game.GameId, request2);

        var stored =
            await _context.PlayerItems.FirstAsync(i =>
                i.InventoryTypeOptionId == Guid.Parse(request.InventoryTypeOptionId));

        Assert.That(stored.Amount, Is.EqualTo(13));
    }

    private async Task CreateTestPlayerInventory()
    {
        var groupId = Guid.NewGuid();
        _context.PlayerItemGroups.Add(new PlayerItemGroup
        {
            PlayerId = Guid.Parse(_user.UserId),
            InventoryTypeId = Guid.Parse(_inventoryType.InventoryTypeId),
            Id = groupId
        });

        _context.PlayerItems.Add(new PlayerItem
        {
            Amount = 5,
            InventoryTypeOptionId = _context.InventoryTypeOptions.First().Id,
            PlayerItemGroupId = groupId
        });

        await _context.SaveChangesAsync();
    }

    private UpdatePlayerInventoryRequest MakeUpdateRequest(int amount = 1)
    {
        return new UpdatePlayerInventoryRequest
        {
            PlayerId = _user.UserId,
            InventoryTypeOptionId = _context.InventoryTypeOptions.First().Id.ToString(),
            Amount = amount
        };
    }
}
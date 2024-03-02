using Database.Common.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Database.MySql.Test;

public class MySqlDataStorer_InventoryTypes_Tests : BaseDbTest
{
    private string _gameId { get; set; }

    [SetUp]
    public async Task CreateUserAndGame()
    {
        var user = await _repository.CreateUser("username", "password");
        var game = await _repository.CreateGame("game", user.UserId);

        _gameId = game.GameId;
    }

    private CreateInventoryTypeDTO GetCreateInventoryTypeRequest()
    {
        return new CreateInventoryTypeDTO
        {
            Name = "type",
            Options = new List<InventoryTypeOption>
            {
                new()
                {
                    Label = "label",
                    Value = "value"
                }
            }
        };
    }

    private async Task<InventoryTypeResponse> CreateInventoryType()
    {
        var request = GetCreateInventoryTypeRequest();

        var result = await _repository.CreateInventoryType(_gameId, request);
        return result;
    }

    [Test]
    public async Task CanCreateInventoryType()
    {
        var result = await CreateInventoryType();

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task CreateActuallyCreatesInTheDb()
    {
        var result = await CreateInventoryType();

        var stored = _context.InventoryTypes
            .Include(i => i.Options)
            .First(i => i.Id == Guid.Parse(result.InventoryTypeId));

        Assert.That(stored, Is.Not.Null);
        Assert.That(stored.GameId, Is.EqualTo(Guid.Parse(result.GameId)));
        Assert.That(stored.Name, Is.EqualTo(result.Name));
        Assert.That(stored.Options.Count(), Is.EqualTo(1));
        Assert.That(stored.Options.First().Label, Is.EqualTo(result.Options.First().Label));
        Assert.That(stored.Options.First().Value, Is.EqualTo(result.Options.First().Value));
    }

    [Test]
    public async Task CanGetInventoryTypeById()
    {
        var result = await CreateInventoryType();

        var stored = await _repository.GetInventoryTypeById(result.InventoryTypeId);

        Assert.That(stored, Is.Not.Null);
        Assert.That(stored.GameId, Is.EqualTo(result.GameId));
        Assert.That(stored.Name, Is.EqualTo(result.Name));
        Assert.That(stored.Options.Count(), Is.EqualTo(1));
        Assert.That(stored.Options.First().Label, Is.EqualTo(result.Options.First().Label));
        Assert.That(stored.Options.First().Value, Is.EqualTo(result.Options.First().Value));
    }

    [Test]
    public async Task ReturnsNullIfIdIsNotValid()
    {
        var result = await _repository.GetInventoryTypeById(Guid.NewGuid().ToString());

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CanGetInventoryTypesForGame()
    {
        var result = await CreateInventoryType();

        var getByGameResult = await _repository.GetInventoryTypesForGame(result.GameId);

        Assert.That(getByGameResult.Count, Is.EqualTo(1));

        var stored = getByGameResult.First();

        Assert.That(stored, Is.Not.Null);
        Assert.That(stored.GameId, Is.EqualTo(result.GameId));
        Assert.That(stored.Name, Is.EqualTo(result.Name));
        Assert.That(stored.Options.Count(), Is.EqualTo(1));
        Assert.That(stored.Options.First().Label, Is.EqualTo(result.Options.First().Label));
        Assert.That(stored.Options.First().Value, Is.EqualTo(result.Options.First().Value));
    }

    [Test]
    public async Task CanEditTheNameOfInventoryType()
    {
        var result = await CreateInventoryType();

        var editRequest = new EditInventoryTypeDTO
        {
            Name = "edited",
            Options = result.Options,
            InventoryTypeId = result.InventoryTypeId
        };

        var editResponse = await _repository.EditInventoryType(editRequest);

        Assert.That(editResponse.Name, Is.EqualTo("edited"));

        var stored = _context.InventoryTypes.First(i => i.Name == "edited");
        Assert.That(stored, Is.Not.Null);
    }

    [Test]
    public async Task EditingInventoryTypeOptionsSavesOptionsDifferences()
    {
        var result = await CreateInventoryType();

        var editRequest = new EditInventoryTypeDTO
        {
            Name = "edited",
            Options = new List<InventoryTypeOption>
            {
                new()
                {
                    Label = "something",
                    Value = "different"
                },
                new()
                {
                    Label = "something2",
                    Value = "different2"
                }
            },
            InventoryTypeId = result.InventoryTypeId
        };

        var editResponse = await _repository.EditInventoryType(editRequest);

        var stored = _context.InventoryTypes
            .Include(i => i.Options)
            .First(i => i.Name == "edited");
        Assert.That(stored, Is.Not.Null);

        Assert.That(stored.Options.Count(), Is.EqualTo(2));

        Assert.That(stored.Options.First().Label, Is.EqualTo("something"));
        Assert.That(stored.Options.First().Value, Is.EqualTo("different"));
        
        Assert.That(editResponse.Options.Count(), Is.EqualTo(2));

        Assert.That(editResponse.Options.First().Label, Is.EqualTo("something"));
        Assert.That(editResponse.Options.First().Value, Is.EqualTo("different"));
    }
}
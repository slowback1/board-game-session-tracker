using Database.Common.DTOs;
using Database.Common.Game;
using Database.Common.Test.Storers;

namespace Database.Common.Test.Game;

public class InventoryTypeRepositoryTests : BaseDbTest
{
    private InventoryTypeRepository _repository;

    [SetUp]
    public void SetUpRepository()
    {
        _repository = new InventoryTypeRepository(DataStorer);
    }


    [Test]
    public async Task ReturnsTheInventoryTypeWhenCreatingAnInventoryType()
    {
        var created = await _repository.CreateInventoryType("game", new CreateInventoryTypeDTO
        {
            Name = "name",
            Options = new List<InventoryTypeOption> { new() { Value = "value", Label = "label" } }
        });

        Assert.That(created.Name, Is.EqualTo("name"));
        Assert.That(created.GameId, Is.EqualTo("game"));
        Assert.That(created.Options.First().Label, Is.EqualTo("label"));
        Assert.That(created.Options.First().Value, Is.EqualTo("value"));
    }

    [Test]
    public async Task CallsTheDataStorerWhenCreatingAnInventoryType()
    {
        var created = await _repository.CreateInventoryType("game", new CreateInventoryTypeDTO
        {
            Name = "name",
            Options = new List<InventoryTypeOption> { new() { Value = "value", Label = "label" } }
        });

        Assert.That(DataStorer.LastCreatedInventoryType, Is.Not.Null);
    }

    [Test]
    public async Task ReturnsTheEditedInventoryType()
    {
        var edited = await _repository.EditInventoryType(new EditInventoryTypeDTO
        {
            Name = "edited",
            Options = new List<InventoryTypeOption> { new() { Value = "value2", Label = "label" } },
            InventoryTypeId = "1234"
        });

        Assert.That(edited.Name, Is.EqualTo("edited"));
        Assert.That(edited.Options.First().Value, Is.EqualTo("value2"));
    }

    [Test]
    public async Task CanGetAnInventoryTypeById()
    {
        var result = await _repository.GetInventoryTypeById("1234");

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task CanGetInventoryTypesForGame()
    {
        var results = await _repository.GetInventoryTypesForGame("1234");

        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count(), Is.EqualTo(1));
    }
}
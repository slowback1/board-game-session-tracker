using Database.Common.DTOs;
using Database.Common.Game;
using Database.Common.Test.Storers;

namespace Database.Common.Test.Game;

public class GameRepositoryTests : BaseDbTest
{
    private GameRepository _repository;

    [SetUp]
    public void SetUpCreator()
    {
        _repository = new GameRepository(DataStorer);
    }

    [Test]
    public async Task ReturnsTheGameWhenCreatingAGame()
    {
        var result = await _repository.CreateGame(new CreateGameRequest { GameName = "test" }, "user");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Response, Is.Not.Null);
        Assert.That(result.Response.GameName, Is.EqualTo("test"));
    }

    [Test]
    public async Task CreateGameStoresTheGameInTheDatabase()
    {
        var result = await _repository.CreateGame(new CreateGameRequest { GameName = "test" }, "user");

        var stored = DataStorer.LastCreatedGame;

        Assert.That(stored, Is.Not.Null);
        Assert.That(stored!.GameName, Is.EqualTo("test"));
    }

    [Test]
    public async Task CreateGameReturnsAnErrorIfStoringThrowsAnException()
    {
        var result = await _repository.CreateGame(new CreateGameRequest { GameName = "error" }, "username");

        Assert.That(result.Errors, Is.Not.Null);
        Assert.That(result.Errors!.Count, Is.GreaterThan(0));
        Assert.That(result.Errors, Contains.Item("Error creating game."));
    }
}
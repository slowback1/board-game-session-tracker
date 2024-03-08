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

    [Test]
    public async Task CanGetGamesForUser()
    {
        var result = await _repository.GetGamesForUser("test");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Response, Is.Not.Null);
        Assert.That(result.Response.Count, Is.GreaterThan(0));
    }

    [Test]
    public async Task ReturnsTheGameWithTheUserAddedToIt()
    {
        var result = await _repository.AddUserToGame("user", "game");

        Assert.That(result, Is.Not.Null);

        Assert.That(result.Response.Players.Select(p => p.UserId), Contains.Item("user"));
        Assert.That(result.Response, Is.EqualTo(DataStorer.LastCreatedGame));
    }

    [Test]
    public async Task DoesNotAddTheSamePlayerTwiceWhenTryingToAddThePlayerTwice()
    {
        var result = await _repository.AddUserToGame("user", "game");
        var beforeCount = result.Response.Players.Count();
        var result2 = await _repository.AddUserToGame("user", "game");
        var afterCount = result2.Response.Players.Count();

        Assert.That(beforeCount, Is.EqualTo(afterCount));
    }

    [Test]
    public async Task ReturnsAnErrorResultIfGameIsNotFound()
    {
        var result = await _repository.AddUserToGame("user", "null");

        Assert.That(result.Errors, Is.Not.Null);
        Assert.That(result.Errors!.First().ToLower(), Contains.Substring("not found"));
    }

    [Test]
    public async Task ReturnsAnErrorResultIfPlayerAddeningErrors()
    {
        var result = await _repository.AddUserToGame("error", "error");

        Assert.That(result.Errors, Is.Not.Null);
        Assert.That(result.Errors.Count, Is.GreaterThan(0));
    }

    [Test]
    public async Task CanGetGameById()
    {
        var result = await _repository.GetGameById("game");

        Assert.That(result.Response, Is.Not.Null);
        Assert.That(result.Response.GameId, Is.EqualTo(DataStorer.LastCreatedGame.GameId));
    }

    [Test]
    public async Task GetGameByIdReturnsErrorResponseIfNoGameFound()
    {
        var result = await _repository.GetGameById("null");

        Assert.That(result.Errors, Is.Not.Null);
        Assert.That(result.Errors!.First(), Contains.Substring("not found"));
    }
}
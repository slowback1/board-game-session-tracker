using Database.Common.DTOs;
using Database.MySql.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.MySql.Test;

public class MySqlDataStorer_Game_Tests_Create_Game : MySqlDataStorerGameTestBase
{
    [Test]
    public async Task CanCreateAGame()
    {
        var created = await _repository.CreateGame("game", _userId);

        Assert.That(created, Is.Not.Null);
    }

    [Test]
    public async Task ActuallyCreatesTheGameWithTheCorrectInformation()
    {
        await _repository.CreateGame("game", _userId);

        var stored = _context.Games
            .Include(g => g.HostUser)
            .Include(g => g.Players)
            .FirstOrDefault(g => g.GameName == "game");

        Assert.That(stored, Is.Not.Null);
        Assert.That(stored!.GameName, Is.EqualTo("game"));
        Assert.That(stored.HostUserId, Is.EqualTo(Guid.Parse(_userId)));
        Assert.That(stored.Players.Count(), Is.EqualTo(1));
        Assert.That(stored.Players.First().Id, Is.EqualTo(Guid.Parse(_userId)));
    }

    [Test]
    public async Task ReturnsTheGameObject()
    {
        var result = await _repository.CreateGame("game", _userId);

        Assert.That(result.GameName, Is.EqualTo("game"));
        Assert.That(result.GameId, Is.Not.Empty);
        Assert.That(result.Host.UserId, Is.EqualTo(_userId));
        Assert.That(result.Players.Count(), Is.EqualTo(1));
        Assert.That(result.Players.First().UserId, Is.EqualTo(_userId));

        var isGuid = Guid.TryParse(result.GameId, out var _);

        Assert.That(isGuid, Is.True);
    }
}

public abstract class MySqlDataStorerGameTestBase : BaseDbTest
{
    protected User _user { get; set; }
    protected string _userId { get; set; }
    protected Game _game { get; set; }

    [SetUp]
    public async Task CreateUserAndGame()
    {
        var username = "username";
        var createdUser = await _repository.CreateUser(username, "password");

        _user = _context.Users.First(u => u.Username == username);
        _userId = _user.Id.ToString();

        var createdGame = await _repository.CreateGame("game", _userId);
        _game = _context.Games.First(g => g.Id == Guid.Parse(createdGame.GameId));
    }
}

public class MySqlDataStorer_Game_Tests_GetGamesForUser : MySqlDataStorerGameTestBase
{
    [Test]
    public async Task ReturnsAListOfGamesForTheGivenUserId()
    {
        var result = await _repository.GetGamesForUser(_userId);

        Assert.That(result.Count, Is.EqualTo(1));

        var firstItem = result.First();

        Assert.That(firstItem.GameName, Is.EqualTo(_game.GameName));
        Assert.That(firstItem.GameId, Is.EqualTo(_game.Id.ToString()));
        Assert.That(firstItem.Host.UserId, Is.EqualTo(_userId));
        Assert.That(firstItem.Players.Count(), Is.EqualTo(1));
        Assert.That(firstItem.Players.First().UserId, Is.EqualTo(_userId));
    }

    [Test]
    public async Task AlsoReturnsGamesWhereTheUserIsJustAPlayer()
    {
        var secondUser = await _repository.CreateUser("user2", "password");

        await _repository.AddUserToGame(secondUser.UserId, _game.Id.ToString());

        var gameList = await _repository.GetGamesForUser(secondUser.UserId);

        Assert.That(gameList.Count, Is.EqualTo(1));
    }
}

public class MySqlDataStorer_Game_Tests_GameGameById : MySqlDataStorerGameTestBase
{
    [Test]
    public async Task CanGetGameByIdWhenItExists()
    {
        var id = _game.Id.ToString();

        var game = await _repository.GetGameById(id);

        Assert.That(game.GameName, Is.EqualTo(_game.GameName));
    }

    [Test]
    public async Task ReturnsNullIfGameIdDoesntExist()
    {
        var game = await _repository.GetGameById(Guid.NewGuid().ToString());

        Assert.That(game, Is.Null);
    }
}

public class MySqlDataStorer_Game_Tests_AddUserToGame : MySqlDataStorerGameTestBase
{
    private UserDTO secondUser { get; set; }

    [SetUp]
    public async Task CreateASecondUser()
    {
        secondUser = await _repository.CreateUser("user2", "password");
    }

    [Test]
    public async Task AddUserAddsTheUserToTheGameInTheDatabase()
    {
        var result = await _repository.AddUserToGame(secondUser.UserId, _game.Id.ToString());

        var storedGame = _context.Games.Include(g => g.Players).First(g => g.Id == _game.Id);

        Assert.That(storedGame.Players.Select(p => p.Id.ToString()), Contains.Item(secondUser.UserId));
    }

    [Test]
    public async Task AddUserReturnsTheGameWithTheUser()
    {
        var result = await _repository.AddUserToGame(secondUser.UserId, _game.Id.ToString());

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Players.Select(p => p.UserId), Contains.Item(secondUser.UserId));
    }

    [Test]
    public void ThrowsExceptionIfUserIsNull()
    {
        Assert.ThrowsAsync<Exception>(async () =>
            await _repository.AddUserToGame(Guid.NewGuid().ToString(), _game.Id.ToString()));
    }

    [Test]
    public void ThrowsExceptionIfGameIsNull()
    {
        Assert.ThrowsAsync<Exception>(async () =>
            await _repository.AddUserToGame(secondUser.UserId, Guid.NewGuid().ToString()));
    }
}
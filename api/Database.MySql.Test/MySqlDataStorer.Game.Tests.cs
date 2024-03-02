using Database.MySql.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.MySql.Test;

public class MySqlDataStorer_Game_Tests_Create_Game : BaseDbTest
{
    private User _user { get; set; }
    private string _userId { get; set; }

    [SetUp]
    public async Task CreateUserForGame()
    {
        var username = "username";
        var createdUser = await _repository.CreateUser(username, "password");

        _user = _context.Users.First(u => u.Username == username);
        _userId = _user.Id.ToString();
    }

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

public class MySqlDataStorer_Game_Tests_GetGamesForUser : BaseDbTest
{
    private User _user { get; set; }
    private string _userId { get; set; }
    private Game _game { get; set; }

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
}
using Database.Common.User;
using Microsoft.EntityFrameworkCore;

namespace Database.MySql.Test;

public class MySqlDataStorer_User_Tests : BaseDbTest
{
    [Test]
    public async Task CanCreateUser()
    {
        var user = await _repository.CreateUser("username", "password");

        Assert.That(user, Is.Not.Null);
    }

    [Test]
    public async Task CreatedUserHasExpectedInfo()
    {
        var user = await _repository.CreateUser("username", "password");

        Assert.That(user.Username, Is.EqualTo("username"));
        Assert.That(user.CreatedAt, Is.EqualTo(DateTime.Today));
        Assert.That(user.UserId, Is.Not.Empty);


        var canParse = Guid.TryParse(user.UserId, out var guid);

        Assert.That(canParse, Is.True);
    }

    [Test]
    public async Task CreatedUserActuallyCreatesUserInDB()
    {
        var user = await _repository.CreateUser("username", "password");

        var stored = await _context.Users.FirstAsync();

        Assert.That(stored.Username, Is.EqualTo("username"));
        Assert.That(stored.PasswordHash, Is.Not.Empty);
        Assert.That(stored.CreatedAt, Is.EqualTo(DateTime.Today));
        Assert.That(stored.Id, Is.Not.Empty);

        var guid = stored.Id.ToString();

        Assert.That(guid, Is.Not.Empty);
    }
}

public class MySqlDataStorerLoginTests : BaseDbTest
{
    [SetUp]
    public async Task CreateATestUser()
    {
        var hashedPassword = PasswordHasher.Hash("password", 100);

        await _repository.CreateUser("username", hashedPassword);
    }

    [Test]
    public async Task CanLoginWithCorrectPassword()
    {
        var user = await _repository.VerifyLogin("username", "password");

        Assert.That(user, Is.Not.Null);
    }

    [Test]
    public async Task LoggedInUserHasCorrectInfo()
    {
        var user = await _repository.VerifyLogin("username", "password");

        Assert.That(user.Username, Is.EqualTo("username"));
        Assert.That(user.CreatedAt, Is.EqualTo(DateTime.Today));
        Assert.That(user.UserId, Is.Not.Empty);


        var canParse = Guid.TryParse(user.UserId, out var guid);

        Assert.That(canParse, Is.True);
    }

    [Test]
    public async Task ReturnsNullIfGivenInvalidUsername()
    {
        var user = await _repository.VerifyLogin("invalid", "password");

        Assert.That(user, Is.Null);
    }

    [Test]
    public async Task ReturnsNullIfGivenInvalidPassword()
    {
        var user = await _repository.VerifyLogin("username", "invalid");

        Assert.That(user, Is.Null);
    }
}
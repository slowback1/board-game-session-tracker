using Database.Common.DTOs;
using Database.Common.Test.Storers;
using Database.Common.User;

namespace Database.Common.Test.User;

public class UserRepositoryTests : BaseDbTest
{
    private readonly CreateUserDTO testUserToCreate = new()
    {
        Username = "user",
        Password = "password123",
        ConfirmPassword = "password123"
    };

    private UserRepository _repository;

    [SetUp]
    public void SetUpCreator()
    {
        _repository = new UserRepository(DataStorer, "areallyreallyreallyreallylongsigning_key");
    }

    [Test]
    public async Task CreatesAUserWhenGivenValidUsernameAndPassword()
    {
        var userToCreate = testUserToCreate;

        var result = await _repository.CreateUser(userToCreate);

        var lastCreatedUser = DataStorer.LastCreatedUser;

        Assert.That(lastCreatedUser.Username, Is.EqualTo(testUserToCreate.Username));
        Assert.That(lastCreatedUser.Password, Is.Not.EqualTo(testUserToCreate.Password));
    }

    [Test]
    public async Task CreateAUserReturnsTheCreatedUser()
    {
        var result = await _repository.CreateUser(testUserToCreate);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Response.Username, Is.EqualTo(testUserToCreate.Username));
    }

    [Test]
    public async Task CanLoginAndReceiveAToken()
    {
        var result = await _repository.Login(new LoginDTO
        {
            Username = "username",
            Password = "password"
        });

        Assert.That(string.IsNullOrEmpty(result.Response?.Token), Is.False);
    }

    [Test]
    public async Task LoginResultIsAValidJWT()
    {
        var result = await _repository.Login(new LoginDTO
        {
            Username = "username",
            Password = "password"
        });

        Assert.That(result.Response.Token, Contains.Substring("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"));
    }

    [Test]
    public async Task LoginResultContainsUser()
    {
        var result = await _repository.Login(new LoginDTO
        {
            Username = "user",
            Password = "passs"
        });

        Assert.That(result.Response.User.Username, Is.EqualTo("user"));
    }

    [Test]
    public async Task LoginWithWrongPasswordSendsNullResult()
    {
        var result = await _repository.Login(new LoginDTO
        {
            Username = "username",
            Password = "invalid"
        });

        Assert.That(result.Response, Is.Null);
    }

    [Test]
    public async Task LoginWithWrongPasswordSendsErrorMessage()
    {
        var result = await _repository.Login(new LoginDTO
        {
            Username = "username",
            Password = "invalid"
        });

        Assert.That(result.Errors.Count, Is.GreaterThan(0));
        Assert.That(result.Errors, Contains.Item("Invalid username or password."));
    }
}
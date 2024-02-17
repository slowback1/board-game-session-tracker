using Database.Common.DTOs;
using Database.Common.Storers;

namespace Database.Common.Test.Storers;

public class TestDataStorer : IDataStorer
{
    public CreateUserDTO? LastCreatedUser { get; private set; }

    public GameDTO? LastCreatedGame { get; private set; }

    public async Task<UserDTO> CreateUser(string username, string password)
    {
        LastCreatedUser = new CreateUserDTO
        {
            Username = username,
            ConfirmPassword = password,
            Password = password
        };

        return new UserDTO
        {
            Username = username,
            CreatedAt = DateTime.Now,
            UserId = "1234"
        };
    }

    public async Task<UserDTO?> VerifyLogin(string username, string password)
    {
        if (password == "invalid") return null;

        return new UserDTO
        {
            Username = username,
            CreatedAt = DateTime.Now,
            UserId = "1234"
        };
    }

    public async Task<GameDTO> CreateGame(string gameName, string hostUserId)
    {
        if (gameName == "error") throw new Exception("I BROKE IT!!!");

        var hostPlayer = new UserDTO
        {
            Username = "test user",
            CreatedAt = DateTime.Now,
            UserId = hostUserId
        };

        LastCreatedGame = new GameDTO
        {
            Players = new List<UserDTO> { hostPlayer },
            GameName = gameName,
            Host = hostPlayer,
            GameId = "test id"
        };

        return LastCreatedGame;
    }

    public async Task<List<GameDTO>> GetGamesForUser(string userId)
    {
        if (LastCreatedGame is null)
            await CreateGame("test game", userId);

        return new List<GameDTO> { LastCreatedGame! };
    }
}
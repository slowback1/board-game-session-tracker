using Database.Common.DTOs;
using Database.Common.Storers;

namespace Database.Common.Test.Storers;

public class TestDataStorer : IDataStorer
{
    public CreateUserDTO? LastCreatedUser { get; private set; }

    public GameDTO? LastCreatedGame { get; private set; }

    public InventoryTypeResponse LastCreatedInventoryType { get; private set; }

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

    public async Task<InventoryTypeResponse> CreateInventoryType(string gameId, CreateInventoryTypeDTO dto)
    {
        LastCreatedInventoryType = new InventoryTypeResponse
        {
            GameId = gameId,
            Name = dto.Name,
            Options = dto.Options,
            InventoryTypeId = "1234"
        };

        return LastCreatedInventoryType;
    }

    public async Task<InventoryTypeResponse> EditInventoryType(EditInventoryTypeDTO dto)
    {
        if (LastCreatedInventoryType is null)
            await CreateInventoryType("test", dto);

        LastCreatedInventoryType.Name = dto.Name;
        LastCreatedInventoryType.Options = dto.Options;

        return LastCreatedInventoryType;
    }

    public async Task<InventoryTypeResponse> GetInventoryTypeById(string id)
    {
        return new InventoryTypeResponse
        {
            GameId = "1234",
            Name = "test",
            Options = new List<InventoryTypeOption>
            {
                new()
                {
                    Value = "value",
                    Label = "label"
                }
            },
            InventoryTypeId = id
        };
    }

    public async Task<List<InventoryTypeResponse>> GetInventoryTypesForGame(string gameId)
    {
        return new List<InventoryTypeResponse>
        {
            new()
            {
                GameId = gameId,
                Name = "test",
                Options = new List<InventoryTypeOption>
                {
                    new()
                    {
                        Value = "value",
                        Label = "label"
                    }
                },
                InventoryTypeId = "1234"
            }
        };
    }

    public async Task<GameDTO> AddUserToGame(string userId, string gameId)
    {
        if (userId == "error" || gameId == "error")
            throw new Exception("Error!");

        if (LastCreatedGame is null)
            await CreateGame("create me", "not-the-user-id-probably");

        if (LastCreatedUser is null)
            await CreateUser(userId, "password");

        LastCreatedGame!.Players.Add(new UserDTO { UserId = userId, Username = LastCreatedUser!.Username });

        return LastCreatedGame;
    }

    public async Task<GameDTO?> GetGameById(string gameId)
    {
        if (gameId == "null") return null;

        if (LastCreatedGame is null)
            await CreateGame("create me", "user-id");

        return LastCreatedGame!;
    }
}
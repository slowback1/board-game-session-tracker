using Database.Common.DTOs;

namespace Database.Common.Storers;

public interface IDataStorer
{
    Task<UserDTO> CreateUser(string username, string password);
    Task<UserDTO?> VerifyLogin(string username, string password);
    Task<GameDTO> CreateGame(string gameName, string hostUserId);
    Task<List<GameDTO>> GetGamesForUser(string userId);
    Task<InventoryTypeResponse> CreateInventoryType(string gameId, CreateInventoryTypeDTO dto);
    Task<InventoryTypeResponse> EditInventoryType(EditInventoryTypeDTO dto);
    Task<InventoryTypeResponse> GetInventoryTypeById(string id);
    Task<List<InventoryTypeResponse>> GetInventoryTypesForGame(string gameId);
    Task<GameDTO> AddUserToGame(string userId, string gameId);
    Task<GameDTO?> GetGameById(string gameId);
    Task<List<PlayerInventory>?> GetInventoryForGame(string gameId);
    Task<PlayerInventoryItem> UpdateInventoryForPlayer(string gameId, UpdatePlayerInventoryRequest request);
}
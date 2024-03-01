using Database.Common.DTOs;
using Database.Common.Storers;

namespace Database.MySql;

public partial class MySqlDataStorer : IDataStorer
{
    protected readonly ApplicationDbContext _context;

    public MySqlDataStorer(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<GameDTO> CreateGame(string gameName, string hostUserId)
    {
        throw new NotImplementedException();
    }

    public Task<List<GameDTO>> GetGamesForUser(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<InventoryTypeResponse> CreateInventoryType(string gameId, CreateInventoryTypeDTO dto)
    {
        throw new NotImplementedException();
    }

    public Task<InventoryTypeResponse> EditInventoryType(EditInventoryTypeDTO dto)
    {
        throw new NotImplementedException();
    }

    public Task<InventoryTypeResponse> GetInventoryTypeById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<InventoryTypeResponse>> GetInventoryTypesForGame(string gameId)
    {
        throw new NotImplementedException();
    }
}
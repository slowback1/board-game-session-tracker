using Database.Common.DTOs;
using Database.Common.Storers;

namespace Database.Common.Game;

public class InventoryTypeRepository : DbRepository
{
    public InventoryTypeRepository(IDataStorer storer) : base(storer)
    {
    }

    public async Task<InventoryTypeResponse> CreateInventoryType(string gameId, CreateInventoryTypeDTO dto)
    {
        return await _storer.CreateInventoryType(gameId, dto);
    }

    public async Task<InventoryTypeResponse> EditInventoryType(EditInventoryTypeDTO dto)
    {
        return await _storer.EditInventoryType(dto);
    }

    public async Task<InventoryTypeResponse> GetInventoryTypeById(string id)
    {
        return await _storer.GetInventoryTypeById(id);
    }

    public async Task<List<InventoryTypeResponse>> GetInventoryTypesForGame(string gameId)
    {
        return await _storer.GetInventoryTypesForGame(gameId);
    }
}
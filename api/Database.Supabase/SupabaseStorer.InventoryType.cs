using Database.Common.DTOs;
using Database.Common.Storers;
using Database.Supabase.Models;

namespace Database.Supabase;

public partial class SupabaseStorer : IDataStorer
{
    public async Task<InventoryTypeResponse> CreateInventoryType(string gameId, CreateInventoryTypeDTO dto)
    {
        var models = new InventoryTypeObjects(dto);

        var inventoryType = models.InventoryTypes;
        inventoryType.GameId = gameId;

        var createResult = await _client.From<InventoryTypes>().Insert(inventoryType);

        var createdInventoryType = createResult.Model;

        var options = models.Options;
        foreach (var option in options)
            option.InventoryTypeId = createdInventoryType.Id;

        var optionsCreateResult = await _client.From<InventoryTypeOptions>().Insert(options);
        var createdOptions = optionsCreateResult.Models;

        var response = createdInventoryType.ToResponse(createdOptions);

        return response;
    }

    public async Task<InventoryTypeResponse> EditInventoryType(EditInventoryTypeDTO dto)
    {
        var objects = await GetInventoryTypeDatabaseObjectById(dto.InventoryTypeId);

        var inventoryType = objects.InventoryTypes;
        var oldOptions = objects.Options;

        inventoryType.Name = dto.Name;

        foreach (var option in oldOptions)
            await _client.From<InventoryTypeOptions>().Delete(option);

        var newOptions = new InventoryTypeObjects(dto).Options;

        var insertResult = await _client.From<InventoryTypeOptions>().Insert(newOptions);
        await _client.From<InventoryTypes>().Update(inventoryType);

        return inventoryType.ToResponse(insertResult.Models);
    }

    public async Task<InventoryTypeResponse> GetInventoryTypeById(string id)
    {
        var objects = await GetInventoryTypeDatabaseObjectById(id);

        return objects.InventoryTypes.ToResponse(objects.Options);
    }

    public async Task<List<InventoryTypeResponse>> GetInventoryTypesForGame(string gameId)
    {
        var response = await _client.From<InventoryTypes>().Where(t => t.GameId == gameId).Get();
        var objects = response.Models;

        var inventoryTypes = new List<InventoryTypeResponse>();

        foreach (var obj in objects)
        {
            var optionsResponse =
                await _client.From<InventoryTypeOptions>().Where(o => o.InventoryTypeId == obj.Id).Get();
            var options = optionsResponse.Models;

            inventoryTypes.Add(obj.ToResponse(options));
        }

        return inventoryTypes;
    }

    private async Task<InventoryTypeObjects> GetInventoryTypeDatabaseObjectById(string id)
    {
        var inventoryType = await _client.From<InventoryTypes>().Where(t => t.Id == id).Single();
        var optionsResponse = await _client.From<InventoryTypeOptions>().Where(o => o.InventoryTypeId == id).Get();
        var options = optionsResponse.Models;

        if (inventoryType is null) return null;

        return new InventoryTypeObjects
        {
            InventoryTypes = inventoryType,
            Options = options
        };
    }
}
using Database.Common.DTOs;
using Database.MySql.Models;
using Microsoft.EntityFrameworkCore;
using InventoryTypeOption = Database.MySql.Models.InventoryTypeOption;

namespace Database.MySql;

public partial class MySqlDataStorer
{
    public async Task<InventoryTypeResponse> CreateInventoryType(string gameId, CreateInventoryTypeDTO dto)
    {
        var inventoryType = new InventoryType
        {
            GameId = Guid.Parse(gameId),
            Name = dto.Name,
            Options = dto.Options.Select(o => new InventoryTypeOption
            {
                Label = o.Label,
                Value = o.Value
            }).ToList()
        };

        _context.InventoryTypes.Add(inventoryType);

        await _context.SaveChangesAsync();


        return ConvertToDTO(inventoryType);
    }

    public async Task<InventoryTypeResponse> EditInventoryType(EditInventoryTypeDTO dto)
    {
        var stored = await GetInventoryTypeModelById(dto.InventoryTypeId);

        stored.Name = dto.Name;
        stored.Options = GetNewSetOfInventoryTypeOptions(stored, dto.Options);

        await _context.SaveChangesAsync();

        return ConvertToDTO(stored);
    }

    public async Task<InventoryTypeResponse> GetInventoryTypeById(string id)
    {
        var inventoryType = await GetInventoryTypeModelById(id);

        if (inventoryType is null) return null;

        return ConvertToDTO(inventoryType);
    }

    public async Task<List<InventoryTypeResponse>> GetInventoryTypesForGame(string gameId)
    {
        var inventoryTypes = await _context.InventoryTypes
            .Include(i => i.Options)
            .Where(i => i.GameId == Guid.Parse(gameId))
            .ToListAsync();

        return inventoryTypes.Select(ConvertToDTO).ToList();
    }

    private List<InventoryTypeOption> GetNewSetOfInventoryTypeOptions(InventoryType stored,
        List<Common.DTOs.InventoryTypeOption> newSetOfOptions)
    {
        var options = new List<InventoryTypeOption>();

        foreach (var option in stored.Options)
        {
            var matching = newSetOfOptions.FirstOrDefault(o => o.Value == option.Value);

            if (matching is not null)
            {
                option.Label = matching.Label;

                options.Add(option);
            }
        }

        var newOptions = newSetOfOptions
            .Where(o => stored.Options.All(opt => opt.Value != o.Value))
            .Select(o => new InventoryTypeOption
            {
                Label = o.Label,
                Value = o.Value
            });

        options.AddRange(newOptions);

        return options;
    }

    private async Task<InventoryType> GetInventoryTypeModelById(string id)
    {
        var inventoryType = await _context.InventoryTypes
            .Include(i => i.Options)
            .FirstOrDefaultAsync(i => i.Id == Guid.Parse(id));

        return inventoryType;
    }

    private InventoryTypeResponse ConvertToDTO(InventoryType inventoryType)
    {
        return new InventoryTypeResponse
        {
            Name = inventoryType.Name,
            Options = inventoryType.Options.Select(opt => new Common.DTOs.InventoryTypeOption
            {
                Value = opt.Value,
                Label = opt.Label
            }).ToList(),
            GameId = inventoryType.GameId.ToString(),
            InventoryTypeId = inventoryType.Id.ToString()
        };
    }
}
using Database.Common.DTOs;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Database.Supabase.Models;

public class InventoryTypeObjects
{
    public InventoryTypeObjects()
    {
    }

    public InventoryTypeObjects(CreateInventoryTypeDTO dto)
    {
        InventoryTypes = new InventoryTypes
        {
            Name = dto.Name
        };

        MapOptions(dto.Options);
    }

    public InventoryTypeObjects(EditInventoryTypeDTO dto)
    {
        InventoryTypes = new InventoryTypes
        {
            Name = dto.Name,
            Id = dto.InventoryTypeId
        };

        MapOptions(dto.Options);
    }

    public InventoryTypes InventoryTypes { get; set; }
    public List<InventoryTypeOptions> Options { get; set; }


    private void MapOptions(List<InventoryTypeOption> options)
    {
        Options = options.Select(opt => new InventoryTypeOptions
        {
            Value = opt.Value,
            Label = opt.Label,
            InventoryTypeId = InventoryTypes.Id
        }).ToList();
    }
}

[Table("inventory_types")]
public class InventoryTypes : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("game_id")]
    public string GameId { get; set; }

    [Column("name")]
    public string Name { get; set; }

    public InventoryTypeResponse ToResponse(List<InventoryTypeOptions> options)
    {
        return new InventoryTypeResponse
        {
            Name = Name,
            GameId = GameId,
            InventoryTypeId = Id,
            Options = options.Select(opt => new InventoryTypeOption
            {
                Label = opt.Label,
                Value = opt.Value
            }).ToList()
        };
    }
}

[Table("inventory_type_options")]
public class InventoryTypeOptions : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("inventory_type_id")]
    public string InventoryTypeId { get; set; }

    [Column("label")]
    public string Label { get; set; }

    [Column("value")]
    public string Value { get; set; }
}
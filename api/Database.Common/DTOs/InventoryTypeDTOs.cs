using Validator.Attributes;

namespace Database.Common.DTOs;

public class CreateInventoryTypeDTO
{
    [Required]
    public string Name { get; set; }

    [Required]
    [Length(min: 1)]
    [UniqueListValue(nameof(InventoryTypeOption.Value), "Option values must be unique!")]
    public List<InventoryTypeOption> Options { get; set; }
}

public class EditInventoryTypeDTO : CreateInventoryTypeDTO
{
    [Required]
    [ValidGuid]
    public string InventoryTypeId { get; set; }
}

public class InventoryTypeResponse
{
    public string GameId { get; set; }
    public string InventoryTypeId { get; set; }
    public string Name { get; set; }
    public List<InventoryTypeOption> Options { get; set; }
}

public class InventoryTypeOption
{
    [Required]
    public string Label { get; set; }

    [Required]
    public string Value { get; set; }
}
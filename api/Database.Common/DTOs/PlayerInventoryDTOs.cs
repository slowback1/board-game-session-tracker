using Validator.Attributes;

namespace Database.Common.DTOs;

public class PlayerInventoryItem
{
    public string InventoryTypeOptionId { get; set; }
    public string Name { get; set; }
    public int Amount { get; set; }
}

public class PlayerInventoryItemGroup
{
    public string InventoryTypeName { get; set; }
    public string InventoryTypeId { get; set; }
    public List<PlayerInventoryItem> Items { get; set; }
}

public class PlayerInventory
{
    public string PlayerName { get; set; }
    public string PlayerId { get; set; }
    public List<PlayerInventoryItemGroup> Inventory { get; set; }
}

public class UpdatePlayerInventoryRequest
{
    [Required]
    [ValidGuid]
    public string PlayerId { get; set; }

    [Required]
    [ValidGuid]
    public string InventoryTypeOptionId { get; set; }

    [Required]
    public int Amount { get; set; }
}

public class UpdateAllPlayerInventoriesRequest
{
    [Required]
    [ValidGuid]
    public string InventoryTypeOptionId { get; set; }

    [Required]
    public int AmountChanged { get; set; }
}
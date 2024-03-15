using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.MySql.Models;

public class PlayerItem
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("PlayerItemGroup")]
    public Guid PlayerItemGroupId { get; set; }

    [ForeignKey("InventoryTypeOption")]
    public Guid InventoryTypeOptionId { get; set; }

    public InventoryTypeOption Option { get; set; }

    public int Amount { get; set; }
}

public class PlayerItemGroup
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("InventoryType")]
    public Guid InventoryTypeId { get; set; }

    [ForeignKey("User")]
    public Guid PlayerId { get; set; }

    public ICollection<PlayerItem> Items { get; set; }
}
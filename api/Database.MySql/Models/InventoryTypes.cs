using System.ComponentModel.DataAnnotations.Schema;

namespace Database.MySql.Models;

public class InventoryType
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Game Game { get; set; }
    public string Name { get; set; }
    public ICollection<InventoryTypeOption> Options { get; set; }
}

public class InventoryTypeOption
{
    public Guid Id { get; set; }

    [ForeignKey("InventoryTypes")]
    public Guid InventoryTypeId { get; set; }

    public InventoryType InventoryType { get; set; }

    public string Label { get; set; }
    public string Value { get; set; }
}
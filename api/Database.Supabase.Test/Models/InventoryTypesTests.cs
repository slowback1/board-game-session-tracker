using Database.Common.DTOs;
using Database.Supabase.Models;

namespace Database.Supabase.Test.Models;

public class InventoryTypesTests
{
    [Test]
    public void CanConvertToTheResponseObject()
    {
        var type = new InventoryTypes
        {
            Id = "1234",
            GameId = "12345",
            Name = "test"
        };

        var options = new List<InventoryTypeOptions>
        {
            new()
            {
                InventoryTypeId = "1234",
                Id = "1234",
                Label = "label",
                Value = "value"
            }
        };

        var converted = type.ToResponse(options);

        Assert.That(converted.Name, Is.EqualTo("test"));
        Assert.That(converted.InventoryTypeId, Is.EqualTo("1234"));
        Assert.That(converted.GameId, Is.EqualTo("12345"));
        Assert.That(converted.Options.Count(), Is.EqualTo(1));

        var firstOption = converted.Options.First();

        Assert.That(firstOption.Label, Is.EqualTo("label"));
        Assert.That(firstOption.Value, Is.EqualTo("value"));
    }

    [Test]
    public void CanConvertToDatabaseObjectsFromCreateRequest()
    {
        var request = new CreateInventoryTypeDTO
        {
            Options = new List<InventoryTypeOption>
            {
                new()
                {
                    Label = "label",
                    Value = "value"
                }
            },
            Name = "name"
        };

        var converted = new InventoryTypeObjects(request);

        Assert.That(converted.Options.Count(), Is.EqualTo(1));
        var firstOption = converted.Options.First();
        Assert.That(firstOption.Label, Is.EqualTo("label"));
        Assert.That(firstOption.Value, Is.EqualTo("value"));

        var type = converted.InventoryTypes;

        Assert.That(type.Name, Is.EqualTo("name"));
    }

    [Test]
    public void CanConvertToDatabaseObjectFromEditRequest()
    {
        var request = new EditInventoryTypeDTO
        {
            Options = new List<InventoryTypeOption>
            {
                new()
                {
                    Label = "label",
                    Value = "value"
                }
            },
            Name = "name",
            InventoryTypeId = "1234"
        };
        var converted = new InventoryTypeObjects(request);

        Assert.That(converted.Options.Count(), Is.EqualTo(1));
        var firstOption = converted.Options.First();
        Assert.That(firstOption.Label, Is.EqualTo("label"));
        Assert.That(firstOption.Value, Is.EqualTo("value"));

        var type = converted.InventoryTypes;

        Assert.That(type.Name, Is.EqualTo("name"));
        Assert.That(type.Id, Is.EqualTo("1234"));
    }
}
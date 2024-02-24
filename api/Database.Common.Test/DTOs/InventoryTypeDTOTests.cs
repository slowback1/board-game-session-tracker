using Database.Common.DTOs;
using TestUtilities;
using Validator.Attributes;

namespace Database.Common.Test.DTOs;

public class InventoryTypeDTOTests
{
    [Test]
    public void CreateInventoryTypeDTONameIsRequired()
    {
        typeof(CreateInventoryTypeDTO)
            .HasProperty(nameof(CreateInventoryTypeDTO.Name))
            .PropertyHasAttribute<RequiredAttribute>();
    }

    [Test]
    public void CreateInventoryTypeDTOOptionsHasCorrectValidation()
    {
        typeof(CreateInventoryTypeDTO)
            .HasProperty(nameof(CreateInventoryTypeDTO.Options))
            .PropertyHasAttribute<RequiredAttribute>()
            .PropertyHasAttribute<LengthAttribute>();
    }

    [Test]
    public void InventoryTypeOptionLabelIsRequired()
    {
        typeof(InventoryTypeOption)
            .HasProperty(nameof(InventoryTypeOption.Label))
            .PropertyHasAttribute<RequiredAttribute>();
    }

    [Test]
    public void InventoryTypeOptionValueIsRequired()
    {
        typeof(InventoryTypeOption)
            .HasProperty(nameof(InventoryTypeOption.Value))
            .PropertyHasAttribute<RequiredAttribute>();
    }

    [Test]
    public void EditInventoryTypeDTOHasCorrectNameValidation()
    {
        typeof(EditInventoryTypeDTO)
            .HasProperty(nameof(EditInventoryTypeDTO.Name))
            .PropertyHasAttribute<RequiredAttribute>();
    }

    [Test]
    public void EditInventoryTypeDTOHasCorrectOptionsValidation()
    {
        typeof(EditInventoryTypeDTO)
            .HasProperty(nameof(EditInventoryTypeDTO.Options))
            .PropertyHasAttribute<RequiredAttribute>()
            .PropertyHasAttribute<LengthAttribute>();
    }

    [Test]
    public void EditInventoryTypeDTOHasCorrectIdValidation()
    {
        typeof(EditInventoryTypeDTO)
            .HasProperty(nameof(EditInventoryTypeDTO.InventoryTypeId))
            .PropertyHasAttribute<RequiredAttribute>();
    }
}
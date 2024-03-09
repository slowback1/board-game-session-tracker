using Database.Common.DTOs;
using TestUtilities;
using Validator.Attributes;

namespace Database.Common.Test.DTOs;

public class PlayerInventoryDTOTests
{
    [Test]
    public void UpdatePlayerInventoryRequestHasCorrectPlayerIdValidation()
    {
        typeof(UpdatePlayerInventoryRequest)
            .HasProperty(nameof(UpdatePlayerInventoryRequest.PlayerId))
            .PropertyHasAttribute<RequiredAttribute>()
            .PropertyHasAttribute<ValidGuidAttribute>();
    }

    [Test]
    public void UpdatePlayerInventoryRequestHasCorrectInventoryTypeOptionIdValidation()
    {
        typeof(UpdatePlayerInventoryRequest)
            .HasProperty(nameof(UpdatePlayerInventoryRequest.InventoryTypeOptionId))
            .PropertyHasAttribute<RequiredAttribute>()
            .PropertyHasAttribute<ValidGuidAttribute>();
    }

    [Test]
    public void UpdatePlayerInventoryRequestHasCorrectAmountValidation()
    {
        typeof(UpdatePlayerInventoryRequest)
            .HasProperty(nameof(UpdatePlayerInventoryRequest.Amount))
            .PropertyHasAttribute<RequiredAttribute>();
    }

    [Test]
    public void UpdateAllPlayerInventoriesRequestHasCorrectInventoryTypeOptionIdValidation()
    {
        typeof(UpdateAllPlayerInventoriesRequest)
            .HasProperty(nameof(UpdateAllPlayerInventoriesRequest.InventoryTypeOptionId))
            .PropertyHasAttribute<RequiredAttribute>()
            .PropertyHasAttribute<ValidGuidAttribute>();
    }

    [Test]
    public void UpdateAllPlayerInventoriesRequestHasCorrectAmountValidation()
    {
        typeof(UpdateAllPlayerInventoriesRequest)
            .HasProperty(nameof(UpdateAllPlayerInventoriesRequest.AmountChanged))
            .PropertyHasAttribute<RequiredAttribute>();
    }
}
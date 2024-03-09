using Database.Common.DTOs;
using Database.Common.Storers;

namespace Database.Common.Game;

public class PlayerInventoryRepository : DbRepository
{
    public PlayerInventoryRepository(IDataStorer storer) : base(storer)
    {
    }

    public async Task<ApiResponse<List<PlayerInventory>>> GetPlayerInventoryForGame(string gameId)
    {
        var result = await _storer.GetInventoryForGame(gameId);

        if (result is null)
            return Error<List<PlayerInventory>>("Game not found.");

        return Success(result);
    }

    public async Task<ApiResponse<PlayerInventoryItem>> UpdatePlayerInventoryItem(string gameId,
        UpdatePlayerInventoryRequest request)
    {
        var result = await _storer.UpdateInventoryForPlayer(gameId, request);

        return Success(result);
    }

    public async Task<ApiResponse<List<PlayerInventoryItem>>> UpdateAllPlayersInventories(string gameId,
        UpdateAllPlayerInventoriesRequest request)
    {
        var inventory = await _storer.GetInventoryForGame(gameId);

        if (inventory is null) return Error<List<PlayerInventoryItem>>("Game not found.");

        var updatedItems = new List<PlayerInventoryItem>();

        foreach (var player in inventory)
            updatedItems.Add(await UpdatePlayerInventory(gameId, request, player));

        return Success(updatedItems);
    }

    private async Task<PlayerInventoryItem> UpdatePlayerInventory(string gameId,
        UpdateAllPlayerInventoriesRequest updateAllRequest,
        PlayerInventory inventory)
    {
        var amount = GetCurrentInventoryAmount(inventory, updateAllRequest.InventoryTypeOptionId);

        var request = new UpdatePlayerInventoryRequest
        {
            PlayerId = inventory.PlayerId,
            Amount = amount + updateAllRequest.AmountChanged,
            InventoryTypeOptionId = updateAllRequest.InventoryTypeOptionId
        };

        return await _storer.UpdateInventoryForPlayer(gameId, request);
    }

    private int GetCurrentInventoryAmount(PlayerInventory inventory, string inventoryOptionId)
    {
        var amount = 0;

        foreach (var group in inventory.Inventory)
        {
            var item = group.Items.FirstOrDefault(i => i.InventoryTypeOptionId == inventoryOptionId);

            if (item is null) continue;

            amount = item.Amount;
            break;
        }

        return amount;
    }
}
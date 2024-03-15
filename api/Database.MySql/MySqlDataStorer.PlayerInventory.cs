using Database.Common.DTOs;
using Database.MySql.Models;
using Microsoft.EntityFrameworkCore;
using InventoryTypeOption = Database.MySql.Models.InventoryTypeOption;

namespace Database.MySql;

public partial class MySqlDataStorer
{
    public async Task<List<PlayerInventory>?> GetInventoryForGame(string gameId)
    {
        var game = await GetGameEntityById(gameId);

        var inventories = new List<PlayerInventory>();

        foreach (var player in game.Players)
            inventories.Add(new PlayerInventory
            {
                PlayerId = player.Id.ToString(),
                PlayerName = player.Username,
                Inventory = await GetPlayerInventory(game.Id, player.Id)
            });

        return inventories;
    }

    public async Task<PlayerInventoryItem> UpdateInventoryForPlayer(string gameId, UpdatePlayerInventoryRequest request)
    {
        var option = _context.InventoryTypeOptions.First(o => o.Id == Guid.Parse(request.InventoryTypeOptionId));

        var existingGroup = await _context.PlayerItemGroups.Include(i => i.Items)
            .FirstOrDefaultAsync(g =>
                g.PlayerId == Guid.Parse(request.PlayerId) && g.Items.Any(i =>
                    i.InventoryTypeOptionId == Guid.Parse(request.InventoryTypeOptionId)));


        if (existingGroup is null)
            CreateNewPlayerInventoryGroup(request, option);
        else
            UpdatePlayerInventoryGroup(request, existingGroup);

        await _context.SaveChangesAsync();

        return HandleUpdateInventoryForPlayerResponse(request, option);
    }

    private static void UpdatePlayerInventoryGroup(UpdatePlayerInventoryRequest request, PlayerItemGroup existingGroup)
    {
        var item = existingGroup.Items.FirstOrDefault(o =>
            o.InventoryTypeOptionId == Guid.Parse(request.InventoryTypeOptionId));

        if (item is null)
            existingGroup.Items.Add(new PlayerItem
            {
                Amount = request.Amount,
                InventoryTypeOptionId = Guid.Parse(request.InventoryTypeOptionId)
            });
        else
            item.Amount = request.Amount;
    }

    private void CreateNewPlayerInventoryGroup(UpdatePlayerInventoryRequest request, InventoryTypeOption option)
    {
        var newGroup = new PlayerItemGroup
        {
            PlayerId = Guid.Parse(request.PlayerId),
            InventoryTypeId = option.InventoryTypeId,
            Items = new List<PlayerItem>
            {
                new()
                {
                    Amount = request.Amount,
                    InventoryTypeOptionId = Guid.Parse(request.InventoryTypeOptionId)
                }
            }
        };

        _context.PlayerItemGroups.Add(newGroup);
    }

    private PlayerInventoryItem HandleUpdateInventoryForPlayerResponse(UpdatePlayerInventoryRequest request,
        InventoryTypeOption option)
    {
        return new PlayerInventoryItem
        {
            Amount = request.Amount,
            InventoryTypeOptionId = option.Id.ToString(),
            Name = option.Label
        };
    }

    private async Task<List<PlayerInventoryItemGroup>> GetPlayerInventory(Guid gameId, Guid playerId)
    {
        var types = await _context.InventoryTypes
            .Include(t => t.Options)
            .Where(i => i.GameId == gameId)
            .ToListAsync();


        var groups = new List<PlayerInventoryItemGroup>();

        foreach (var t in types)
        {
            var playerInventory = await _context.PlayerItemGroups
                .Include(g => g.Items)
                .FirstOrDefaultAsync(g => g.InventoryTypeId == t.Id && g.PlayerId == playerId);

            var group = new PlayerInventoryItemGroup
            {
                InventoryTypeName = t.Name,
                InventoryTypeId = t.Id.ToString(),
                Items = t.Options.Select(o => new PlayerInventoryItem
                {
                    Amount = GetAmountFromPlayerGroup(o.Id, playerInventory),
                    InventoryTypeOptionId = o.Id.ToString(),
                    Name = o.Label
                }).ToList()
            };
            groups.Add(group);
        }

        return groups;
    }

    private int GetAmountFromPlayerGroup(Guid optionId, PlayerItemGroup? group)
    {
        if (group is null) return 0;

        var option = group.Items.FirstOrDefault(i => i.InventoryTypeOptionId == optionId);

        return option?.Amount ?? 0;
    }
}
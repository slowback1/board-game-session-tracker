using API.Attributes;
using Database.Common.DTOs;
using Database.Common.Game;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authenticated]
[Route("PlayerInventory")]
public class PlayerInventoryController : BaseController
{
    private readonly PlayerInventoryRepository _repository;

    public PlayerInventoryController(IConfiguration config) : base(config)
    {
        _repository = new PlayerInventoryRepository(Storer);
    }

    [HttpGet]
    [Route("{gameId}")]
    public async Task<ApiResponse<List<PlayerInventory>>> GetPlayerInventoryForGame(string gameId)
    {
        return await _repository.GetPlayerInventoryForGame(gameId);
    }

    [HttpPut]
    [Route("{gameId}/SinglePlayer")]
    public async Task<ApiResponse<PlayerInventoryItem>> UpdateSinglePlayerInventory(string gameId,
        [FromBody] UpdatePlayerInventoryRequest request)
    {
        return await _repository.UpdatePlayerInventoryItem(gameId, request);
    }

    [HttpPut]
    [Route("{gameId}/AllPlayers")]
    public async Task<ApiResponse<List<PlayerInventoryItem>>> UpdateAllPlayerInventories(string gameId,
        [FromBody] UpdateAllPlayerInventoriesRequest request)
    {
        return await _repository.UpdateAllPlayersInventories(gameId, request);
    }
}
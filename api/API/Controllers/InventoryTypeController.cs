using API.Attributes;
using Database.Common.DTOs;
using Database.Common.Game;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authenticated]
[Route("InventoryTypes")]
public class InventoryTypeController : BaseController
{
    private readonly InventoryTypeRepository _repository;

    public InventoryTypeController(IConfiguration config) : base(config)
    {
        _repository = new InventoryTypeRepository(Storer);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<InventoryTypeResponse> GetInventoryTypeById(string id)
    {
        return await _repository.GetInventoryTypeById(id);
    }

    [HttpPost]
    [Route("Create/{gameId}")]
    public async Task<InventoryTypeResponse> CreateInventoryType(string gameId, [FromBody] CreateInventoryTypeDTO dto)
    {
        return await _repository.CreateInventoryType(gameId, dto);
    }

    [HttpPut]
    [Route("Edit/{id}")]
    public async Task<InventoryTypeResponse> EditInventoryType(string id, [FromBody] EditInventoryTypeDTO dto)
    {
        return await _repository.EditInventoryType(dto);
    }

    [HttpGet]
    [Route("ForGame/{gameId}")]
    public async Task<List<InventoryTypeResponse>> GetInventoryTypesForGame(string gameId)
    {
        return await _repository.GetInventoryTypesForGame(gameId);
    }
}
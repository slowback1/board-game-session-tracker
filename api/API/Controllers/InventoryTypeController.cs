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
    public async Task<ApiResponse<InventoryTypeResponse>> GetInventoryTypeById(string id)
    {
        return new ApiResponse<InventoryTypeResponse>
        {
            Response = await _repository.GetInventoryTypeById(id)
        };
    }

    [HttpPost]
    [Route("Create/{gameId}")]
    public async Task<ApiResponse<InventoryTypeResponse>> CreateInventoryType(string gameId,
        [FromBody] CreateInventoryTypeDTO dto)
    {
        return new ApiResponse<InventoryTypeResponse> { Response = await _repository.CreateInventoryType(gameId, dto) };
    }

    [HttpPut]
    [Route("Edit/{id}")]
    public async Task<ApiResponse<InventoryTypeResponse>> EditInventoryType(string id,
        [FromBody] EditInventoryTypeDTO dto)
    {
        return new ApiResponse<InventoryTypeResponse> { Response = await _repository.EditInventoryType(dto) };
    }

    [HttpGet]
    [Route("ForGame/{gameId}")]
    public async Task<ApiResponse<List<InventoryTypeResponse>>> GetInventoryTypesForGame(string gameId)
    {
        return new ApiResponse<List<InventoryTypeResponse>>
            { Response = await _repository.GetInventoryTypesForGame(gameId) };
    }
}
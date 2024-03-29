﻿using API.Attributes;
using Database.Common.DTOs;
using Database.Common.Game;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
[Authenticated]
public class GameController : BaseController
{
    private readonly GameRepository _repository;

    public GameController(IConfiguration config) : base(config)
    {
        _repository = new GameRepository(Storer);
    }

    [HttpPost]
    [Route("")]
    public async Task<ApiResponse<GameDTO>> CreateGame(CreateGameRequest request)
    {
        return await _repository.CreateGame(request, AuthenticatedUser.UserId);
    }

    [HttpGet]
    [Route("ForUser")]
    public async Task<ApiResponse<List<GameDTO>>> GetGamesForUser()
    {
        return await _repository.GetGamesForUser(AuthenticatedUser.UserId);
    }

    [HttpPost]
    [Route("AddPlayer/{gameId}")]
    public async Task<ApiResponse<GameDTO>> AddSelfToGame(string gameId)
    {
        return await _repository.AddUserToGame(AuthenticatedUser!.UserId, gameId);
    }

    [HttpGet]
    [Route("{gameId}")]
    public async Task<ApiResponse<GameDTO>> GetGameById(string gameId)
    {
        return await _repository.GetGameById(gameId);
    }
}
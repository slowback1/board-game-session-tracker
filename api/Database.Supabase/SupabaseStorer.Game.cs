using Database.Common.DTOs;
using Database.Supabase.Models;

namespace Database.Supabase;

public partial class SupabaseStorer
{
    public async Task<GameDTO> CreateGame(string gameName, string hostUserId)
    {
        var model = new Game
        {
            GameName = gameName,
            HostUserId = hostUserId
        };

        var result = await _client.From<Game>().Insert(model);

        var createdGame = result.Model;

        var gamePlayer = new GamePlayer
        {
            GameId = createdGame.Id,
            PlayerId = hostUserId
        };

        var gamePlayerResult = await _client.From<GamePlayer>().Insert(gamePlayer);

        return await GetGameFromId(createdGame.Id);
    }

    public async Task<List<GameDTO>> GetGamesForUser(string userId)
    {
        var gameResult = await _client.From<GamePlayer>().Where(gp => gp.PlayerId == userId).Get();
        var games = gameResult.Models;

        var gameDtos = new List<GameDTO>();

        foreach (var game in games) gameDtos.Add(await GetGameFromId(game.GameId));

        return gameDtos;
    }

    public Task<GameDTO> AddUserToGame(string userId, string gameId)
    {
        throw new NotImplementedException();
    }

    public Task<GameDTO> GetGameById(string gameId)
    {
        throw new NotImplementedException();
    }

    private async Task<GameDTO> GetGameFromId(string gameId)
    {
        var gameResponse = await _client.From<Game>().Where(g => g.Id == gameId).Get();
        var game = gameResponse.Model;

        var playerResponse = await _client.From<GamePlayer>().Where(g => g.GameId == gameId).Get();
        var players = playerResponse.Models.Select(m => m.PlayerId).ToList();
        players.Add(game.HostUserId);


        var userResponse = await _client.From<User>()
            .Get();
        var users = userResponse.Models
            .Where(u => players.Contains(u.Id));

        var hostUser = users.FirstOrDefault(u => u.Id == game.HostUserId);

        return new GameDTO
        {
            GameId = game.Id,
            GameName = game.GameName,
            Host = hostUser.ToUserDTO(),
            Players = users.Select(u => u.ToUserDTO()).ToList()
        };
    }
}
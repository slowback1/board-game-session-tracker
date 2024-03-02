using Database.Common.DTOs;
using Database.MySql.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.MySql;

public partial class MySqlDataStorer
{
    public async Task<GameDTO> CreateGame(string gameName, string hostUserId)
    {
        var game = new Game
        {
            Players = new List<User>(),
            CreatedAt = DateTime.Today,
            Id = Guid.NewGuid(),
            GameName = gameName,
            HostUserId = Guid.Parse(hostUserId)
        };

        AddUserToGame(game, hostUserId);

        _context.Games.Add(game);

        await _context.SaveChangesAsync();

        return ConvertToDTO(game);
    }

    public async Task<List<GameDTO>> GetGamesForUser(string userId)
    {
        var games = await _context.Games
            .Include(g => g.HostUser)
            .Include(g => g.Players)
            .Where(g => g.HostUserId == Guid.Parse(userId))
            .ToListAsync();

        return games.Select(ConvertToDTO).ToList();
    }

    private GameDTO ConvertToDTO(Game game)
    {
        return new GameDTO
        {
            Players = game.Players.Select(p => ConvertToDTO(p)).ToList(),
            GameName = game.GameName,
            Host = ConvertToDTO(game.HostUser),
            GameId = game.Id.ToString()
        };
    }

    private void AddUserToGame(Game game, string userId)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == Guid.Parse(userId));

        if (game.Players is null)
            game.Players = new List<User>();

        game.Players.Add(user);
    }
}
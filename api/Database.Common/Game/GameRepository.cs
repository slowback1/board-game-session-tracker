using Database.Common.DTOs;
using Database.Common.Storers;

namespace Database.Common.Game;

public class GameRepository : DbRepository
{
    public GameRepository(IDataStorer storer) : base(storer)
    {
    }

    public async Task<ApiResponse<GameDTO>> CreateGame(CreateGameRequest request, string userId)
    {
        GameDTO? result = null;

        try
        {
            result = await _storer.CreateGame(request.GameName, userId);
        }
        catch (Exception E)
        {
            Console.WriteLine(E);
        }

        var errors = GetErrorsForCreateGameResult(result);

        return errors is null ? Success(result!) : Error<GameDTO>(errors);
    }

    public async Task<ApiResponse<List<GameDTO>>> GetGamesForUser(string userId)
    {
        var result = await _storer.GetGamesForUser(userId);

        return Success(result);
    }

    public async Task<ApiResponse<GameDTO>> GetGameById(string gameId)
    {
        var game = await _storer.GetGameById(gameId);

        if (game is null) return Error<GameDTO>("Game not found.");

        return Success(game);
    }

    private List<string>? GetErrorsForCreateGameResult(GameDTO? dto)
    {
        if (dto is null)
            return new List<string>
            {
                "Error creating game."
            };

        return null;
    }

    public async Task<ApiResponse<GameDTO>> AddUserToGame(string userId, string gameId)
    {
        var storedGame = await _storer.GetGameById(gameId);

        if (storedGame is null)
            return Error<GameDTO>("Game not found.");

        if (storedGame.Players.Any(p => p.UserId == userId))
            return Success(storedGame);

        return await TryToAddUserToGame(userId, gameId);
    }

    private async Task<ApiResponse<GameDTO>> TryToAddUserToGame(string userId, string gameId)
    {
        try
        {
            var game = await _storer.AddUserToGame(userId, gameId);

            return Success(game);
        }
        catch (Exception e)
        {
            return Error<GameDTO>(e.Message);
        }
    }
}
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

        return new ApiResponse<GameDTO>
        {
            Response = result,
            Errors = GetErrorsForCreateGameResult(result)
        };
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
}
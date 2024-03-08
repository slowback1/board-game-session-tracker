using Database.Common.DTOs;

namespace Database.Common.Storers;

public abstract class DbRepository
{
    protected DbRepository(IDataStorer storer)
    {
        _storer = storer;
    }

    protected IDataStorer _storer { get; private set; }

    protected ApiResponse<T> Success<T>(T response)
    {
        return new ApiResponse<T> { Response = response };
    }

    protected ApiResponse<T> Error<T>(string error)
    {
        return new ApiResponse<T> { Errors = new List<string> { error } };
    }

    protected ApiResponse<T> Error<T>(List<string> errors)
    {
        return new ApiResponse<T> { Errors = errors };
    }
}
using Database.Common.DTOs;
using Database.Common.Storers;

namespace Database.Common.User;

public class UserRepository : DbRepository
{
    public UserRepository(IDataStorer storer) : base(storer)
    {
    }

    public async Task<ApiResponse<UserDTO>> CreateUser(CreateUserDTO dto)
    {
        var errorResult = ValidateCreateUserDTO(dto);
        if (errorResult != null) return errorResult;

        var result = await _storer.CreateUser(dto.Username, PasswordHasher.Hash(dto.Password));

        return new ApiResponse<UserDTO>
        {
            Response = result
        };
    }

    private ApiResponse<UserDTO>? ValidateCreateUserDTO(CreateUserDTO dto)
    {
        var errors = new List<string>();

        if (dto.Password != dto.ConfirmPassword)
            errors.Add("Passwords must match.");

        if (string.IsNullOrEmpty(dto.Username))
            errors.Add("Username must be filled out.");

        if (string.IsNullOrEmpty(dto.Password))
            errors.Add("Password must be filled out.");

        if (errors.Count > 0)
            return new ApiResponse<UserDTO>
            {
                Errors = errors
            };

        return null;
    }
}
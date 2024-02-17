using Database.Common.DTOs;
using Database.Common.Storers;
using Validator;

namespace Database.Common.User;

public class UserRepository : DbRepository
{
    private readonly string _signingKey;

    public UserRepository(IDataStorer storer, string signingKey) : base(storer)
    {
        _signingKey = signingKey;
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

    public async Task<ApiResponse<LoginResponse>> Login(LoginDTO dto)
    {
        var validationErrors = ObjectValidator.ValidateObject(dto);

        if (validationErrors.Count > 0) return new ApiResponse<LoginResponse> { Errors = validationErrors };

        var result = await _storer.VerifyLogin(dto.Username, dto.Password);

        if (result is null)
            return new ApiResponse<LoginResponse>
            {
                Errors = new List<string>
                {
                    "Invalid username or password."
                }
            };

        return HandleSuccessfulLoginResponse(result);
    }

    private ApiResponse<LoginResponse> HandleSuccessfulLoginResponse(UserDTO result)
    {
        var token = new TokenProvider(_signingKey).GenerateToken(result);

        return new ApiResponse<LoginResponse>
        {
            Response = new LoginResponse
            {
                Token = token,
                User = result
            }
        };
    }

    private ApiResponse<UserDTO>? ValidateCreateUserDTO(CreateUserDTO dto)
    {
        var errors = ObjectValidator.ValidateObject(dto);

        if (errors.Count > 0)
            return new ApiResponse<UserDTO>
            {
                Errors = errors
            };

        return null;
    }
}
using Database.Common.DTOs;
using Database.Common.User;
using Microsoft.AspNetCore.Mvc;

namespace Api.SlowbackBgSession.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : BaseController
{
    private readonly UserRepository _repository;

    public UserController(IConfiguration config) : base(config)
    {
        _repository = new UserRepository(Storer, JwtConfig.Key);
    }

    [HttpPost("CreateUser")]
    public async Task<ApiResponse<UserDTO>> CreateUser(CreateUserDTO dto)
    {
        return await _repository.CreateUser(dto);
    }

    [HttpPost("Login")]
    public async Task<ApiResponse<LoginResponse>> Login(LoginDTO dto)
    {
        return await _repository.Login(dto);
    }

    [HttpGet("")]
    public async Task<ApiResponse<UserDTO>> GetCurrentlyLoggedInUser()
    {
        if (AuthenticatedUser != null)
            return new ApiResponse<UserDTO>
            {
                Response = AuthenticatedUser
            };

        return new ApiResponse<UserDTO>
        {
            Errors = new List<string>
            {
                "Token is either missing or not valid"
            }
        };
    }
}
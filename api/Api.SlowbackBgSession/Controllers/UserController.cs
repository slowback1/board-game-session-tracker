using Api.SlowbackBgSession.Config;
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
        var jwtConfig = config.GetSection("Jwt").Get<JwtConfig>()!;

        _repository = new UserRepository(Storer, jwtConfig.Key);
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
}
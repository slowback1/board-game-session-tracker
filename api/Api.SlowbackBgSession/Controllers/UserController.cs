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
        _repository = new UserRepository(Storer);
    }

    [HttpPost("CreateUser")]
    public async Task<ApiResponse<UserDTO>> CreateUser(CreateUserDTO dto)
    {
        return await _repository.CreateUser(dto);
    }
}
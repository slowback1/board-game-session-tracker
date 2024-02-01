using Database.Common.DTOs;
using Database.Common.Storers;

namespace Database.Common.Test.Storers;

public class TestDataStorer : IDataStorer
{
    public CreateUserDTO LastCreatedUser { get; private set; }

    public async Task<UserDTO> CreateUser(string username, string password)
    {
        LastCreatedUser = new CreateUserDTO
        {
            Username = username,
            ConfirmPassword = password,
            Password = password
        };

        return new UserDTO
        {
            Username = username,
            CreatedAt = DateTime.Now,
            UserId = "1234"
        };
    }
}
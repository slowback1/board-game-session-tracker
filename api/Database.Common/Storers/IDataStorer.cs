using Database.Common.DTOs;

namespace Database.Common.Storers;

public interface IDataStorer
{
    Task<UserDTO> CreateUser(string username, string password);
    Task<UserDTO?> VerifyLogin(string username, string password);
}
using Database.Common.DTOs;
using Database.Common.User;
using Database.MySql.Models;

namespace Database.MySql;

public partial class MySqlDataStorer
{
    public async Task<UserDTO> CreateUser(string username, string password)
    {
        var id = Guid.NewGuid();

        var user = new User
        {
            Username = username,
            CreatedAt = DateTime.Today,
            PasswordHash = password,
            Id = id
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();


        return GetUserByName(username)!;
    }

    public async Task<UserDTO?> VerifyLogin(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user is null) return null;

        var isMatch = PasswordHasher.Verify(password, user.PasswordHash);

        if (!isMatch) return null;

        return GetUserByName(username);
    }

    private UserDTO ConvertToDTO(User user)
    {
        return new UserDTO
        {
            Username = user.Username,
            CreatedAt = user.CreatedAt,
            UserId = user.Id.ToString()
        };
    }

    private UserDTO GetUserByName(string username)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        return ConvertToDTO(user!);
    }
}
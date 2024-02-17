using Database.Common.DTOs;
using Database.Common.User;
using Database.Supabase.Models;

namespace Database.Supabase;

public partial class SupabaseStorer
{
    public async Task<UserDTO> CreateUser(string username, string password)
    {
        await initPromise;

        var model = new User
        {
            UserName = username,
            PasswordHash = password
        };

        var result = await _client.From<User>().Insert(model);
        var createdUser = result.Model;

        return createdUser.ToUserDTO();
    }

    public async Task<UserDTO?> VerifyLogin(string username, string password)
    {
        var user = await _client.From<User>().Where(u => u.UserName == username).Get();

        if (user.Model is null) return null;

        var passwordIsValid = PasswordHasher.Verify(password, user.Model.PasswordHash);

        if (passwordIsValid)
            return user.Model.ToUserDTO();

        return null;
    }
}
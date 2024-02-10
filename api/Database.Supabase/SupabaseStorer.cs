using Database.Common.DTOs;
using Database.Common.Storers;
using Database.Common.User;
using Database.Supabase.Models;
using Supabase;

namespace Database.Supabase;

public class SupabaseStorer : IDataStorer
{
    public SupabaseStorer(string url, string key)
    {
        var options = new SupabaseOptions
        {
            AutoConnectRealtime = true
        };

        _client = new Client(url, key, options);
        initPromise = _client.InitializeAsync();
    }

    private Client _client { get; }
    private Task initPromise { get; }

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

        return new UserDTO
        {
            Username = createdUser.UserName,
            CreatedAt = createdUser.CreatedAt,
            UserId = createdUser.Id
        };
    }

    public async Task<UserDTO?> VerifyLogin(string username, string password)
    {
        var user = await _client.From<User>().Where(u => u.UserName == username).Get();

        if (user.Model is null) return null;

        var passwordIsValid = PasswordHasher.Verify(password, user.Model.PasswordHash);

        if (passwordIsValid)
            return new UserDTO
            {
                Username = username,
                CreatedAt = user.Model.CreatedAt,
                UserId = user.Model.Id
            };

        return null;
    }
}
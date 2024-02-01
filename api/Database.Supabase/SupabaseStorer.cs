using Database.Common.DTOs;
using Database.Common.Storers;
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
}
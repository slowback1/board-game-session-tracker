using Database.Common.Storers;
using Supabase;

namespace Database.Supabase;

public partial class SupabaseStorer : IDataStorer
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
}
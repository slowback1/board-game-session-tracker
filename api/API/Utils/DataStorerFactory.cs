using API.Config;
using Database.Common.Storers;
using Database.MySql;
using Database.Supabase;
using Microsoft.EntityFrameworkCore;

namespace API.Utils;

public class DataStorerFactory
{
    private readonly DataStorerConfig _config;

    public DataStorerFactory(DataStorerConfig config)
    {
        _config = config;
    }

    public SupabaseConfig SupabaseConfig { private get; set; }
    public DatabaseConfig DatabaseConfig { private get; set; }

    public IDataStorer GetDataStorerFromConfig()
    {
        switch (_config.Use)
        {
            case "Supabase":
                return GetSupaBaseStorer();
            case "EntityFramework":
                return GetMySqlStorer();
        }

        throw new InvalidOperationException("Given Invalid 'Use' Value in Application Settings");
    }

    private SupabaseStorer GetSupaBaseStorer()
    {
        return new SupabaseStorer(SupabaseConfig.Url, SupabaseConfig.Key);
    }

    private MySqlDataStorer GetMySqlStorer()
    {
        var context = new ApplicationDbContext(DatabaseConfig.ConnectionString);

        if (!string.IsNullOrEmpty(DatabaseConfig.ConnectionString))
            context.Database.Migrate();


        return new MySqlDataStorer(context);
    }
}
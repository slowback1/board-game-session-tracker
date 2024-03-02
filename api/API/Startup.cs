using API.Config;
using Database.MySql;
using Microsoft.EntityFrameworkCore;

namespace API;

public static class Startup
{
    public static async Task MigrateDatabase(IConfiguration configuration)
    {
        var dataStorerConfig = configuration.GetSection("DataStorer").Get<DataStorerConfig>()!;
        if (dataStorerConfig.Use != "EntityFramework")
            return;

        var databaseConfig = configuration.GetSection("Database").Get<DatabaseConfig>()!;

        var context = new ApplicationDbContext(databaseConfig.ConnectionString);
        await context.Database.MigrateAsync();
    }
}
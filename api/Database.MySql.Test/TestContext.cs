using Microsoft.EntityFrameworkCore;

namespace Database.MySql.Test;

public class TestContext : ApplicationDbContext
{
    public TestContext() : base("")
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var randomString = new Random().NextDouble().ToString();
        optionsBuilder.UseInMemoryDatabase(randomString);
    }
}
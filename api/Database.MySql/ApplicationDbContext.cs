using Database.MySql.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Database.MySql;

public class ApplicationDbContext : DbContext
{
    private readonly string _connectionString;

    public ApplicationDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Game> Games { get; set; }
    public virtual DbSet<InventoryType> InventoryTypes { get; set; }
    public virtual DbSet<InventoryTypeOption> InventoryTypeOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Games)
            .WithMany(g => g.Players);

        modelBuilder.Entity<User>()
            .HasMany(u => u.HostGames)
            .WithOne(g => g.HostUser);

        modelBuilder.Entity<InventoryType>()
            .HasMany(i => i.Options)
            .WithOne(o => o.InventoryType);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(new MySqlConnection(_connectionString));
        base.OnConfiguring(optionsBuilder);
    }
}
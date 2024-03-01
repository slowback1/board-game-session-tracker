namespace Database.MySql.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Game> Games { get; set; }
    public ICollection<Game> HostGames { get; set; }
}
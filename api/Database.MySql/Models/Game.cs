using System.ComponentModel.DataAnnotations.Schema;

namespace Database.MySql.Models;

public class Game
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string GameName { get; set; }

    [ForeignKey("User")]
    public Guid HostUserId { get; set; }

    public User HostUser { get; set; }

    public ICollection<User> Players { get; set; }
}
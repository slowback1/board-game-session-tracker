using Postgrest.Attributes;
using Postgrest.Models;

namespace Database.Supabase.Models;

[Table("games")]
public class Game : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("game_name")]
    public string GameName { get; set; }

    [Column("host_user_id")]
    public string HostUserId { get; set; }
}
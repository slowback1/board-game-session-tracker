using Postgrest.Attributes;
using Postgrest.Models;

namespace Database.Supabase.Models;

[Table("game_players")]
public class GamePlayer : BaseModel
{
    [Column("game_id")]
    public string GameId { get; set; }

    [Column("player_id")]
    public string PlayerId { get; set; }

    [PrimaryKey("id")]
    public int Id { get; set; }
}
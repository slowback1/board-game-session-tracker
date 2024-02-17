using Database.Common.DTOs;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Database.Supabase.Models;

[Table("users")]
public class User : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("username")]
    public string UserName { get; set; }

    [Column("password_hash")]
    public string PasswordHash { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    public UserDTO ToUserDTO()
    {
        return new UserDTO
        {
            CreatedAt = CreatedAt,
            UserId = Id,
            Username = UserName
        };
    }
}
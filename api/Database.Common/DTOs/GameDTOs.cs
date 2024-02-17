using Validator.Attributes;

namespace Database.Common.DTOs;

public class CreateGameRequest
{
    [Required]
    [Length(80, 4)]
    public string GameName { get; set; }
}

public class GameDTO
{
    public string GameId { get; set; }
    public string GameName { get; set; }
    public List<UserDTO> Players { get; set; }
    public UserDTO Host { get; set; }
}
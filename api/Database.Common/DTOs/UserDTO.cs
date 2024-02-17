using Validator.Attributes;

namespace Database.Common.DTOs;

public class UserDTO
{
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Username { get; set; }
}

[MatchingProperty(nameof(Password), nameof(ConfirmPassword), "Passwords must match.")]
public class CreateUserDTO
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public string ConfirmPassword { get; set; }
}

public class LoginDTO
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
    public UserDTO User { get; set; }
}
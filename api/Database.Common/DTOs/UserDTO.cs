namespace Database.Common.DTOs;

public class UserDTO
{
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Username { get; set; }
}

public class CreateUserDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    
}
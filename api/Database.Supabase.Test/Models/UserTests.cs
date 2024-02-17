using Database.Supabase.Models;

namespace Database.Supabase.Test.Models;

public class UserTests
{
    [Test]
    public void CanMapUserToDTO()
    {
        var createdTime = DateTime.Now;

        var user = new User
        {
            UserName = "test",
            CreatedAt = createdTime,
            PasswordHash = "hashed",
            Id = "1234"
        };

        var dto = user.ToUserDTO();

        Assert.That(dto.CreatedAt, Is.EqualTo(createdTime));
        Assert.That(dto.Username, Is.EqualTo("test"));
        Assert.That(dto.UserId, Is.EqualTo("1234"));
    }
}
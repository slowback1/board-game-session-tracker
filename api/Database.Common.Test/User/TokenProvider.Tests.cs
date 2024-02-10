using Database.Common.DTOs;
using Database.Common.User;

namespace Database.Common.Test.User;

public class TokenProvider_Tests
{
    [Test]
    public void CanCreateAndDecodeOwnToken()
    {
        var tokenProvider = new TokenProvider("a_really_really_really_really_really_really_long_key");

        var token = tokenProvider.GenerateToken(new UserDTO
        {
            Username = "User",
            CreatedAt = new DateTime(),
            UserId = "1234"
        });

        var validated = tokenProvider.DecodeToken(token);

        Assert.That(validated.Username, Is.EqualTo("User"));
        Assert.That(validated.UserId, Is.EqualTo("1234"));
        Assert.That(validated.CreatedAt, Is.EqualTo(new DateTime()));
    }

    [Test]
    public void DecodingAnInvalidTokenReturnsNull()
    {
        var validated =
            new TokenProvider("a_really_really._really_really_really_really_long_key").DecodeToken(
                "a_really_really_really_really_really_.really_long_key");

        Assert.That(validated, Is.Null);
    }
}
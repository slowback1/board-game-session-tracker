using Database.MySql.Models;

namespace Database.MySql.Test;

public class TestContextTests
{
    [Test]
    public async Task TestContextFunctionsCorrectly()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "string",
            PasswordHash = "hash",
            CreatedAt = new DateTime()
        };

        var context = new TestContext();
        context.Users.Add(user);

        await context.SaveChangesAsync();

        var stored = context.Users.First();

        Assert.That(stored.PasswordHash, Is.EqualTo(user.PasswordHash));
    }
}
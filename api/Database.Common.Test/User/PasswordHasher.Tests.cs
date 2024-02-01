using Database.Common.User;

namespace Database.Common.Test.User;


public class PasswordHasher_Tests
{
    [Test]
    public void HashResultIsNotNull()
    {
        var result = PasswordHasher.Hash("password");
        
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void CanVerifyAHashedPasswordCorrectly()
    {
        var password = "password123";
        var hash = PasswordHasher.Hash(password);

        var verified = PasswordHasher.Verify(password, hash);
        
        Assert.That(verified, Is.EqualTo(true));
    }

    [Test]
    public void CanVerifythatPasswordDoesNotMatch()
    {
        var password = "password123";
        var password2 = "password456";

        var hash = PasswordHasher.Hash(password);
        var verified = PasswordHasher.Verify(password2, hash);

        Assert.That(verified, Is.EqualTo(false));
    }
}
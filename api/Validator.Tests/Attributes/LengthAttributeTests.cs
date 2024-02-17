using Validator.Attributes;

namespace Validator.Tests.Attributes;

public class LengthAttributeTests
{
    [Test]
    public void ThrowsExceptionWhenGivenNonStringValue()
    {
        var attribute = new LengthAttribute(5);

        Assert.Throws<InvalidOperationException>(() =>
            attribute.CheckForValidationError(new LengthAttribute(5))
        );
    }

    [Test]
    public void ReturnsNullWhenGivenAStringWithinTheMaxLengthConstraints()
    {
        var attribute = new LengthAttribute(5);

        var result = attribute.CheckForValidationError("pugi");

        Assert.That(result, Is.Null);
    }

    [Test]
    public void ReturnsNullWhenGivenNullValue()
    {
        var attribute = new LengthAttribute(5);

        var result = attribute.CheckForValidationError(null);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void ReturnsErrorWhenStringIsBiggerThanMaxLength()
    {
        var attribute = new LengthAttribute(5);

        var result = attribute.CheckForValidationError("pugiiii");

        Assert.That(result, Is.EqualTo("'pugiiii' is longer than 5 characters."));
    }

    [Test]
    public void ReturnsErrorWhenStringIsSmallerThanMinLength()
    {
        var attribute = new LengthAttribute(5, 3);

        var result = attribute.CheckForValidationError("p");

        Assert.That(result, Is.EqualTo("'p' is shorter than 3 characters."));
    }
}
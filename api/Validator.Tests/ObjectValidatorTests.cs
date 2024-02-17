﻿using Validator.Attributes;

namespace Validator.Tests;

public class TestObject
{
    [Length(5)]
    [Required]
    public string Value { get; set; }
}

[MatchingProperty(nameof(A), nameof(B))]
public class MatchingPropertyTest
{
    public string A { get; set; }
    public string B { get; set; }
}

public class TestEmpty
{
    public string A { get; set; }
}

public class ObjectValidatorTests
{
    [Test]
    public void ReturnsEmptyListWhenGivenAValidObject()
    {
        var test = new TestObject { Value = "test" };

        var result = ObjectValidator.ValidateObject(test);

        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public void ReturnsListOfValidationErrorsWhenGivenInvalidObject()
    {
        var test = new TestObject { Value = "really long" };

        var result = ObjectValidator.ValidateObject(test);

        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    public void ReturnsAnEmptyListForANullObject()
    {
        TestObject test = null;

        var result = ObjectValidator.ValidateObject(test);

        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public void AssignsPropertyNameToPropetyNameValidationAttributes()
    {
        var test = new TestObject { Value = "" };

        var result = ObjectValidator.ValidateObject(test);

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result.First(), Contains.Substring("Value"));
    }

    [Test]
    public void CanPickUpOnClassLevelAttributes()
    {
        var test = new MatchingPropertyTest
        {
            A = "a",
            B = "b"
        };

        var result = ObjectValidator.ValidateObject(test);

        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    public void ReturnsEmptyListForAClassWithNoValidationAttributes()
    {
        var test = new TestEmpty { A = "value" };

        var result = ObjectValidator.ValidateObject(test);

        Assert.That(result.Count, Is.EqualTo(0));
    }
}
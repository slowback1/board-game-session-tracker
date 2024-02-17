namespace Validator.Attributes;

public abstract class ValidationAttribute : Attribute
{
    public abstract string? CheckForValidationError(object? value);
}
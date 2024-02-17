namespace Validator.Attributes;

public abstract class PropertyNameValidationAttribute : ValidationAttribute
{
    public string PropertyName { get; set; }
}
namespace Validator.Attributes;

public class LengthAttribute : ValidationAttribute
{
    private readonly int _maxLength;
    private readonly int _minLength;

    public LengthAttribute(int maxLength, int minLength = 0)
    {
        _maxLength = maxLength;
        _minLength = minLength;
    }

    public override string? CheckForValidationError(object? value)
    {
        if (value is null) return null;
        if (value.GetType() != typeof(string))
            throw new InvalidOperationException();

        var stringValue = value.ToString();
        var length = stringValue.Length;

        if (length > _maxLength)
            return $"'{stringValue}' is longer than {_maxLength} characters.";
        if (length < _minLength)
            return $"'{stringValue}' is shorter than {_minLength} characters.";

        return null;
    }
}
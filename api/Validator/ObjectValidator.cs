using System.Diagnostics.CodeAnalysis;
using Validator.Attributes;

namespace Validator;

public static class ObjectValidator
{
    public static List<string> ValidateObject<T>(T? input)
    {
        if (input is null) return new List<string>();

        var propertiesToValidate =
            GetValidationAttributesFromProperties(input).Concat(GetValidationAttributesFromClass(input));

        var result = ValidateEachAttribute<T>(propertiesToValidate);

        return result;
    }

    private static List<string> ValidateEachAttribute<T>(IEnumerable<PropertyAttributeValuePair> propertiesToValidate)
    {
        var result = new List<string>();

        foreach (var property in propertiesToValidate)
        {
            var attributes = property.CustomAttributes;
            foreach (var attribute in attributes)
            {
                var validation = attribute.CheckForValidationError(property.Value);
                if (validation != null) result.Add(validation);
            }
        }

        return result;
    }

    private static IEnumerable<PropertyAttributeValuePair> GetValidationAttributesFromClass<T>([DisallowNull] T input)
    {
        var attributes = input.GetType().GetCustomAttributes(typeof(ValidationAttribute), true)
            .Cast<ValidationAttribute>();

        return new List<PropertyAttributeValuePair>
        {
            new()
            {
                Value = input,
                CustomAttributes = attributes
            }
        };
    }

    private static IEnumerable<PropertyAttributeValuePair> GetValidationAttributesFromProperties<T>(
        [DisallowNull] T input)
    {
        var properties = input.GetType().GetProperties();
        var propertiesToValidate = properties.Select(p =>
                new PropertyAttributeValuePair
                {
                    CustomAttributes = p.GetCustomAttributes(typeof(ValidationAttribute), true)
                        .Cast<ValidationAttribute>()
                        .Select(a =>
                        {
                            if (a is PropertyNameValidationAttribute propertyNameValidationAttribute)
                            {
                                propertyNameValidationAttribute.PropertyName = p.Name;
                                return propertyNameValidationAttribute;
                            }

                            return a;
                        }),
                    Value = p.GetValue(input)
                })
            .Where(v => v.CustomAttributes.Any());
        return propertiesToValidate;
    }

    private class PropertyAttributeValuePair
    {
        public IEnumerable<ValidationAttribute> CustomAttributes { get; set; }
        public object? Value { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace API.ValidationAttributes;

public class IsWithinBoundsAttribute : ValidationAttribute
{

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Invalid type. This attribute can't be null!");
        }

        if (value is not int intValue)
        {
            return new ValidationResult("Invalid type. This attribute supports integers only!");
        }

        if (intValue < 0 || intValue > 7)
        {
            return new ValidationResult("The value must be between 0 and 7.");
        }

        return ValidationResult.Success!;
    }

}

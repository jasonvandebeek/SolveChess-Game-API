using SolveChess.Logic.Chess.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SolveChess.API.ValidationAttributes;

public class IsPromotionTypeAttribute : ValidationAttribute
{

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string stringValue)
        {
            return new ValidationResult("Invalid type. This attribute supports strings only!");
        }

        if (Enum.TryParse(stringValue, out PieceType _))
        {
            return ValidationResult.Success!;
        }

        return new ValidationResult("Invalid type given. Cannot be parsed to piece type!");
    }

}

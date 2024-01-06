using SolveChess.API.DTO;
using SolveChess.API.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace SolveChess.API.DTO;

public class OutgoingMoveDto
{

    [Required]
    public int Number { get; set; }

    [Required]
    public string Side { get; set; } = null!;

    [Required]
    public string Notation { get; set; } = null!;

    [Required]
    public SquareDto From { get; set; } = null!;

    [Required]
    public SquareDto To { get; set; } = null!;

    [IsPromotionType]
    public string? Promotion { get; set; }

}

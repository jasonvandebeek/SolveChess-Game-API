using SolveChess.API.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using SolveChess.API.DTO;

namespace SolveChess.API.Models;

public class MoveDataDto
{

    [Required]
    public SquareDto From { get; set; } = null!;

    [Required]
    public SquareDto To { get; set; } = null!;

    [IsPromotionType]
    public string? Promotion { get; set; }

}






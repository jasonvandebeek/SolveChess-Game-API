using SolveChess.API.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace SolveChess.API.Models;

public class MoveDataModel
{

    [Required]
    public Square From { get; set; } = null!;

    [Required]
    public Square To { get; set; } = null!;

    [IsPromotionType]
    public string? Promotion { get; set; }

}

public class Square
{

    [IsWithinBounds]
    public int Rank { get; set; }

    [IsWithinBounds]
    public int File { get; set; }

}




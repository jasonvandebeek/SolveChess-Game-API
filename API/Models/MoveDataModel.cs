using API.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace SolveChess.API.Models;

public class MoveDataModel
{

    [Required]
    public Square From { get; set; } = null!;

    [Required]
    public Square To { get; set; } = null!;

}

public class Square
{

    [IsWithinBounds]
    public int Rank { get; set; }

    [IsWithinBounds]
    public int File { get; set; }

}


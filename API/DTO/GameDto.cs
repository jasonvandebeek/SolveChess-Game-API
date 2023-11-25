
using System.ComponentModel.DataAnnotations;

namespace SolveChess.API.DTO;

public class GameDto
{

    [Required]
    public string Id { get; set; } = null!;

    [Required]
    public string WhitePlayerId { get; set; } = null!;
    [Required]
    public string BlackPlayerId { get; set; } = null!;

    [Required]
    public string State { get; set; } = null!;

    [Required]
    public string Fen { get; set; } = null!;

    [Required]
    public string SideToMove { get; set; } = null!;

    public bool CastlingRightBlackKingSide { get; set; }
    public bool CastlingRightBlackQueenSide { get; set; }
    public bool CastlingRightWhiteKingSide { get; set; }
    public bool CastlingRightWhiteQueenSide { get; set; }

    public SquareDto? EnpassantSquare { get; set; }

}

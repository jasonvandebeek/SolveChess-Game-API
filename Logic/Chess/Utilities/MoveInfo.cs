
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;
using System.ComponentModel.DataAnnotations;

namespace SolveChess.Logic.Chess.Utilities;

public class MoveInfo
{

    [Required]
    public PieceBase Piece { get; set; } = null!;

    public PieceBase? TargetPiece { get; set; }

    [Required]
    public Square From { get; set; } = null!;
    [Required]
    public Square To { get; set; } = null!;

    public PieceType? Promotion { get; set; }

    public bool IsCheck { get; set; }
    public bool IsMate { get; set; }

    public Square? EnpassantSquare { get; set; }

}


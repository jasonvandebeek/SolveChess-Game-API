
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SolveChess.Logic.Models;

public class GameStateModel
{

    [Required]
    public GameState State { get; set; }

    [Required]
    public string Fen { get; set; } = null!;

    public bool CastlingRightBlackKingSide { get; set; }
    public bool CastlingRightBlackQueenSide { get; set; }
    public bool CastlingRightWhiteKingSide { get; set; }
    public bool CastlingRightWhiteQueenSide { get; set; }

    public Square? EnpassantSquare { get; set; }

    [Required]
    public int FullMoveNumber { get; set; }
    [Required]
    public int HalfMoveClock { get; set; }
    [Required]
    public Side SideToMove { get; set; }


}


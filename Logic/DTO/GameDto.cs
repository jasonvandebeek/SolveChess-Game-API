
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.DTO;

public class GameDto
{

    public string Id { get; set; } = null!;

    public string WhitePlayerId { get; set; } = null!;
    public string BlackPlayerId { get; set; } = null!;

    public GameState State { get; set; }

    public string Fen { get; set; } = null!;

    public int FullMoveNumber { get; set; }
    public int HalfMoveClock { get; set; }

    public Side SideToMove { get; set; } 

    public bool CastlingRightBlackKingSide { get; set; }
    public bool CastlingRightBlackQueenSide { get; set; }
    public bool CastlingRightWhiteKingSide { get; set; }
    public bool CastlingRightWhiteQueenSide { get; set; }

    public Square? EnpassantSquare { get; set; }

} 


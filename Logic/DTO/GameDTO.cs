
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.DTO;

public class GameDTO
{

    public string Id { get; set; }

    public string WhitePlayerId { get; set; }
    public string BlackPlayerId { get; set; }

    public GameState State { get; set; }

    public string Fen { get; set; }

    public int FullMoveNumber { get; set; }
    public int HalfMoveClock { get; set; }

    public Side SideToMove { get; set; } 

    public bool CastlingRightBlackKingSide { get; set; }
    public bool CastlingRightBlackQueenSide { get; set; }
    public bool CastlingRightWhiteKingSide { get; set; }
    public bool CastlingRightWhiteQueenSide { get; set; }

    public Square? EnpassantSquare { get; set; }

} 


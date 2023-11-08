
using SolveChess.Logic.Chess.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolveChess.DAL.Model;

public class Game
{

    [Key]
    public string Id { get; set; }

    [ForeignKey("WhitePlayerId")]
    public User WhitePlayer { get; set; }
    public string WhitePlayerId { get; set; }

    [ForeignKey("BlackPlayerId")]
    public User BlackPlayer { get; set; }
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

    public int? EnpassantSquareRank { get; set; }
    public int? EnpassantSquareFile { get; set; }

}


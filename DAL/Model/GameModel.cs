﻿
using SolveChess.Logic.Chess.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SolveChess.DAL.Model;

public class GameModel
{

    [Key]
    public string Id { get; set; } = null!;

    [Required]
    public string WhiteSideUserId { get; set; } = null!;
    [Required]
    public string BlackSideUserId { get; set; } = null!;

    public GameState State { get; set; }

    [Required]
    public string Fen { get; set; } = null!;

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


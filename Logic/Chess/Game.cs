﻿
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.Models;
using SolveChess.Logic.Exceptions;

namespace SolveChess.Logic.Chess;

public class Game
{

    public GameState State { get; private set; }

    private readonly Board board;
    public string Fen { get { return board.Fen; } }

    public int FullMoveNumber { get; private set; }
    public int HalfMoveClock { get; private set; }

    public Side SideToMove { get; private set; }

    public bool CastlingRightBlackKingSide { get { return board.CastlingRightBlackKingSide; } }
    public bool CastlingRightBlackQueenSide { get { return board.CastlingRightBlackQueenSide; } }
    public bool CastlingRightWhiteKingSide { get { return board.CastlingRightWhiteKingSide; } }
    public bool CastlingRightWhiteQueenSide { get { return board.CastlingRightWhiteQueenSide; } }

    public Square? EnpassantSquare { get { return board.EnpassantSquare; } }

    public Game(GameStateModel gameState)
    {
        State = gameState.State;
        board = new Board(gameState.Fen, gameState.CastlingRightBlackKingSide, gameState.CastlingRightBlackQueenSide, gameState.CastlingRightWhiteKingSide, gameState.CastlingRightWhiteQueenSide, gameState.EnpassantSquare);
        FullMoveNumber = gameState.FullMoveNumber;
        HalfMoveClock = gameState.HalfMoveClock;
        SideToMove = gameState.SideToMove;
    }

    public Move? PlayMove(Square from, Square to, PieceType? promotion)
    {
        PieceBase? piece = board.GetPieceAt(from);
        if (State != GameState.IN_PROGRESS || piece == null || piece.Side != SideToMove || !piece.CanMoveToSquare(to, board))
            return null;

        UpdateEnpassantSquare(piece, from, to);
        UpdateCastlingRights(piece, SideToMove, from);

        PieceBase? targetPiece = board.GetPieceAt(to);
        UpdateHalfMoveClock(piece, targetPiece);

        int fullMoveNumber = FullMoveNumber;
        UpdateFullMoveNumber();

        MovePiece(piece, from, to, promotion);

        UpdateCastlingRightsPostMove();

        SwitchSideToMove();
        UpdateGameState();

        var moveInfo = new MoveInfo()
        {
            Piece = piece,
            TargetPiece = targetPiece,
            From = from,
            To = to,
            Promotion = promotion,
            IsCheck = board.KingInCheck(SideToMove), 
            IsMate = board.KingIsMated(SideToMove),
            EnpassantSquare = EnpassantSquare
        };

        string moveNotation = new NotationBuilder(moveInfo).Notation;

        return new Move(fullMoveNumber, piece.Side, moveNotation);
    }

    private void UpdateHalfMoveClock(PieceBase piece, PieceBase? targetPiece)
    {
        HalfMoveClock += 1;

        if (targetPiece != null || piece.Type == PieceType.PAWN)
            HalfMoveClock = 0;
    }

    private void UpdateFullMoveNumber()
    {
        if (SideToMove == Side.BLACK)
            FullMoveNumber += 1;
    }

    private void UpdateGameState()
    {
        if (board.KingIsMated(SideToMove))
        {
            State = SideToMove == Side.BLACK ? GameState.WHITE_VICTORY : GameState.BLACK_VICTORY;
        }
        else if (board.KingInDraw(SideToMove) || HalfMoveClock >= 100)
        {
            State = GameState.DRAW;
        }
    }

    private void SwitchSideToMove()
    {
        SideToMove = SideToMove == Side.BLACK ? Side.WHITE : Side.BLACK;
    }

    private void MovePiece(PieceBase piece, Square from, Square to, PieceType? promotion)
    {
        if (MoveIsPromotion(piece, to))
        {
            if (promotion == null)
                throw new PromotionException("Promotion expected!");

            PromotePiece(from, to, (PieceType)promotion);
        }
        else if (IsCastlingMove(piece, from, to))
        {
            MakeCastlingMove(from, to);
        }
        else
        {
            board.MovePiece(from, to);
        }
            
    }

    private void MakeCastlingMove(Square from, Square to)
    {
        if (to.File == 2)
        {
            board.MovePiece(from, to);
            board.MovePiece(new Square(from.Rank, 0), new Square(to.Rank, 3));
        }
        else if (to.File == 6)
        {
            board.MovePiece(from, to);
            board.MovePiece(new Square(from.Rank, 7), new Square(to.Rank, 5));
        }
    }

    private static bool IsCastlingMove(PieceBase piece, Square from, Square to)
    {
        var isKing = piece.Type == PieceType.KING;
        var isOnBackRank = from.Rank == 0 || from.Rank == 7;
        var isToCastlingFile = to.File == 2 || to.File == 6;

        return isKing && isOnBackRank && isToCastlingFile;
    }

    private void PromotePiece(Square from, Square to, PieceType promotion)
    {
        if (!PromotionTypeIsValid(promotion))
            throw new PromotionException("Invalid promotion piece!");

        board.PromotePiece(from, to, promotion);
    }

    private static bool MoveIsPromotion(PieceBase piece, Square to)
    {
        return piece.Type == PieceType.PAWN && (piece.Side == Side.WHITE && to.Rank == 0 || piece.Side == Side.BLACK && to.Rank == 7);
    }

    private static bool PromotionTypeIsValid(PieceType? promotion)
    {
        return (promotion != PieceType.PAWN && promotion != PieceType.KING);
    }

    private void UpdateEnpassantSquare(PieceBase piece, Square from, Square to)
    {
        var oppositeMoveDirection = piece.Side == Side.WHITE ? 1 : -1;
        if (PawnDoesStartJump(piece, from, to))
            board.EnpassantSquare = new Square(to.Rank + oppositeMoveDirection, to.File);
    }

    private static bool PawnDoesStartJump(PieceBase piece, Square from, Square to)
    {
        return piece.Type == PieceType.PAWN && (piece.Side == Side.WHITE && from.Rank == 6 || piece.Side == Side.BLACK && from.Rank == 1) && Math.Abs(from.Rank - to.Rank) == 2;
    }

    private void UpdateCastlingRights(PieceBase piece, Side side, Square from)
    {
        if (piece.Type == PieceType.KING)
            UpdateCastlingRightsKingMove(side);
        else if (piece.Type == PieceType.ROOK)
            UpdateCastlingRightsRookMove(side, from);
    }

    private void UpdateCastlingRightsKingMove(Side side)
    {
        if (side == Side.BLACK)
        {
            board.CastlingRightBlackQueenSide = false;
            board.CastlingRightBlackKingSide = false;
        }
        else
        {
            board.CastlingRightWhiteQueenSide = false;
            board.CastlingRightWhiteKingSide = false;
        }
    }

    private void UpdateCastlingRightsRookMove(Side side, Square from)
    {
        if(side == Side.BLACK)
        {
            if (from.Equals(new Square(0, 0)))
                board.CastlingRightBlackQueenSide = false;
            else if (from.Equals(new Square(0, 7)))
                board.CastlingRightBlackKingSide = false;
        }
        else
        {
            if (from.Equals(new Square(7, 0)))
                board.CastlingRightWhiteQueenSide = false;
            else if (from.Equals(new Square(7, 7)))
                board.CastlingRightWhiteKingSide = false;
        }
    }

    private void UpdateCastlingRightsPostMove()
    {
        var opposingSide = SideToMove == Side.WHITE ? Side.BLACK : Side.WHITE;

        if (!board.KingInCheck(opposingSide))
            return;

        if (opposingSide == Side.BLACK)
        {
            board.CastlingRightBlackKingSide = false;
            board.CastlingRightBlackQueenSide = false;
        }
        else
        {
            board.CastlingRightWhiteKingSide = false;
            board.CastlingRightWhiteQueenSide = false;
        }
    }

}


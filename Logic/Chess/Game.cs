
using Logic.Chess.Utilities;
using SolveChess.Logic.Attributes;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Factories;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.Models;
using SolveChess.Logic.Exceptions;
using SolveChess.Logic.ResultObjects;

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

    public MoveResult PlayMove(Square from, Square to, PieceType? promotion)
    {
        PieceBase? piece = board.GetPieceAt(from);
        if (piece == null)
            return new MoveResult(StatusCode.FAILURE, "No piece found at that square!");

        if (piece.Side != SideToMove)
            return new MoveResult(StatusCode.FAILURE, "User can't move pieces of that side!");

        if (piece.CanMoveToSquare(to, board))
            return new MoveResult(StatusCode.FAILURE, "Invalid move!");

        UpdateEnpassantSquare(piece, to);
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

        var move = new Move(fullMoveNumber, piece.Side, moveNotation);

        return new MoveResult(StatusCode.SUCCESS, move);
    }

    private void UpdateHalfMoveClock(PieceBase piece, PieceBase? targetPiece)
    {
        if (targetPiece == null && piece.Type != PieceType.PAWN)
            HalfMoveClock += 1;
        else
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
        if (piece.Type == PieceType.PAWN && (piece.Side == Side.WHITE && to.Rank == 0 || piece.Side == Side.BLACK && to.Rank == 7))
        {
            if (promotion == PieceType.PAWN || promotion == PieceType.KING)
                throw new PromotionException("Invalid promotion piece!");

            board.PromotePiece(from, to, promotion ?? throw new PromotionException("Invalid promotion type!"));
        }
        else
            board.MovePiece(from, to);
    }

    private void UpdateEnpassantSquare(PieceBase piece, Square to)
    {
        if (piece.Type == PieceType.PAWN)
        {
            var moveDirection = piece.Side == Side.WHITE ? -1 : 1;
            var pos = board.GetSquareOfPiece(piece);
            if (piece.Side == Side.WHITE && pos.Rank == 6 || piece.Side == Side.BLACK && pos.Rank == 1)
                board.EnpassantSquare = new Square(to.Rank, to.File + moveDirection);
        }
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
            if (from.Equals(new Square(7, 0)))
                board.CastlingRightWhiteQueenSide = false;
            else if (from.Equals(new Square(7, 7)))
                board.CastlingRightWhiteKingSide = false;
        }
        else
        {
            if (from.Equals(new Square(0, 0)))
                board.CastlingRightWhiteQueenSide = false;
            else if (from.Equals(new Square(0, 7)))
                board.CastlingRightWhiteKingSide = false;
        }
    }

    private void UpdateCastlingRightsPostMove()
    {
        var opposingSide = SideToMove == Side.WHITE ? Side.BLACK : Side.WHITE;

        if(board.KingInCheck(opposingSide))
        {
            if(opposingSide == Side.BLACK)
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

}


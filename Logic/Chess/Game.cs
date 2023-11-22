
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Factories;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.DTO;
using SolveChess.Logic.Exceptions;

namespace SolveChess.Logic.Chess;

public class Game
{

    public GameState State { get; private set; }

    private readonly Board board;
    public string Fen { get { return board.Fen; } }

    public int FullMoveNumber { get; private set; } = 0;
    public int HalfMoveClock { get; private set; } = 0;

    public Side SideToMove { get; private set; }

    public bool CastlingRightBlackKingSide { get { return board.CastlingRightBlackKingSide; } }
    public bool CastlingRightBlackQueenSide { get { return board.CastlingRightBlackQueenSide; } }
    public bool CastlingRightWhiteKingSide { get { return board.CastlingRightWhiteKingSide; } }
    public bool CastlingRightWhiteQueenSide { get { return board.CastlingRightWhiteQueenSide; } }

    public Square? EnpassantSquare { get { return board.EnpassantSquare; } }

    public Game(GameDto gameDTO)
    {
        State = gameDTO.State;
        board = new Board(gameDTO.Fen, gameDTO.CastlingRightBlackKingSide, gameDTO.CastlingRightBlackQueenSide, gameDTO.CastlingRightWhiteKingSide, gameDTO.CastlingRightWhiteQueenSide, gameDTO.EnpassantSquare);
        FullMoveNumber = gameDTO.FullMoveNumber;
        HalfMoveClock = gameDTO.HalfMoveClock;
        SideToMove = gameDTO.SideToMove;
    }

    public MoveDto? PlayMove(Square from, Square to, PieceType? promotion)
    {
        PieceBase? piece = board.GetPieceAt(from);
        if (piece == null || piece.Side != SideToMove || board.CanPieceMoveTo(piece, to))
            return null;

        UpdateEnpassantSquare(piece, to);
        UpdateCastlingRights(piece, from);

        PieceBase? targetPiece = board.GetPieceAt(to);
        UpdateHalfMoveClock(piece, targetPiece);

        int fullMoveNumber = FullMoveNumber;
        UpdateFullMoveNumber();

        MovePiece(piece, from, to, promotion);

        UpdateCastlingRightsPostMove();

        SwitchSideToMove();
        UpdateGameState();

        string moveNotation = BuildMoveNotation(piece, targetPiece, from, to, promotion);

        return new MoveDto()
        {
            Number = fullMoveNumber,
            Side = piece.Side,
            Notation = moveNotation
        };
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
            if (promotion != null)
            {
                if (promotion == PieceType.PAWN || promotion == PieceType.KING)
                    throw new PromotionException("Invalid promotion piece!");

                var promotionPiece = PieceFactory.BuildPiece(promotion, piece.Side);

                board.PromotePiece(from, to, promotionPiece);
            }
            else
                throw new PromotionException("Promotion expected!");
        }
        else
            board.MovePiece(from, to);
    }

    private string BuildMoveNotation(PieceBase piece, PieceBase? targetPiece, Square from, Square to, PieceType? promotion)
    {
        string notation = "";

        if (piece.Type == PieceType.PAWN && targetPiece != null)
        {
            notation += "e";
        }
        else
        {
            notation += piece.Notation;
        }

        if (targetPiece != null)
            notation += "x";

        notation += to.Notation;

        if (piece.Type == PieceType.KING)
        {
            if ((from.Rank == 0 || from.Rank == 7) && to.File == 1)
                notation = "O-O-O";
            else if ((from.Rank == 0 || from.Rank == 7) && to.File == 6)
                notation = "O-O";
        }

        if (promotion != null)
        {
            var tempPiece = PieceFactory.BuildPiece(promotion, piece.Side);

            notation += $"={tempPiece.Notation}";
        }

        if (board.KingIsMated(SideToMove))
            notation += "#";
        else if (board.KingInCheck(SideToMove))
            notation += "+";

        if (piece.Type == PieceType.PAWN && to.Equals(board.EnpassantSquare))
            notation += " e.p.";

        return notation;
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

    private void UpdateCastlingRights(PieceBase piece, Square from)
    {
        if (SideToMove == Side.WHITE && (board.CastlingRightWhiteQueenSide || board.CastlingRightWhiteKingSide))
        {
            if (piece.Type == PieceType.KING)
            {
                board.CastlingRightWhiteQueenSide = false;
                board.CastlingRightWhiteKingSide = false;
            }
            else if (piece.Type == PieceType.ROOK)
            {
                if (from.Rank == 7 && from.File == 0)
                    board.CastlingRightWhiteQueenSide = false;
                else if (from.Rank == 7 && from.File == 7)
                    board.CastlingRightWhiteKingSide = false;
            }
        }
        else if (SideToMove == Side.BLACK && (board.CastlingRightBlackQueenSide || board.CastlingRightBlackKingSide))
        {
            if (piece.Type == PieceType.KING)
            {
                board.CastlingRightBlackQueenSide = false;
                board.CastlingRightBlackKingSide = false;
            }
            else if (piece.Type == PieceType.ROOK)
            {
                if (from.Rank == 0 && from.File == 0)
                    board.CastlingRightWhiteQueenSide = false;
                else if (from.Rank == 0 && from.File == 7)
                    board.CastlingRightWhiteKingSide = false;
            }
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



using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Factories;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.DTO;

namespace SolveChess.Logic.Chess;

public class Game
{

    public GameState State { get; private set; } = GameState.IN_PROGRESS;

    private readonly Board board;
    public string Fen { get { return board.Fen; } }

    public int FullMoveNumber { get; private set; } = 0;
    public int HalfMoveClock { get; private set; } = 0;

    public Side SideToMove { get; private set; } = Side.WHITE;

    public bool CastlingRightBlackKingSide { get; private set; } = true;
    public bool CastlingRightBlackQueenSide { get; private set; } = true;
    public bool CastlingRightWhiteKingSide { get; private set; } = true;
    public bool CastlingRightWhiteQueenSide { get; private set; } = true;

    public Square? EnpassantSquare { get; private set; } = null;

    public Game(GameDTO gameDTO)
    {
        State = gameDTO.State;
        board = new Board(gameDTO.Fen);
        FullMoveNumber = gameDTO.FullMoveNumber;
        HalfMoveClock = gameDTO.HalfMoveClock;
        SideToMove = gameDTO.SideToMove;
        CastlingRightBlackKingSide = gameDTO.CastlingRightBlackKingSide;
        CastlingRightBlackQueenSide = gameDTO.CastlingRightBlackQueenSide;
        CastlingRightWhiteKingSide = gameDTO.CastlingRightWhiteKingSide;
        CastlingRightWhiteQueenSide = gameDTO.CastlingRightWhiteQueenSide;
        EnpassantSquare = gameDTO.EnpassantSquare;
    }

    public MoveDTO PlayMove(Square from, Square to, PieceType? promotion)
    {
        PieceBase? piece = board.GetPieceAt(from);
        if (piece == null || piece.Side != SideToMove)
            throw new Exception("Invalid Piece!");

        if (board.CanPieceMoveTo(piece, to))
            throw new Exception("Invalid Move!");

        UpdateEnpassantSquare(piece, to);
        UpdateCastlingRights(piece, from);

        PieceBase? targetPiece = board.GetPieceAt(to);
        UpdateHalfMoveClock(piece, targetPiece);

        int fullMoveNumber = FullMoveNumber;
        UpdateFullMoveNumber();

        MovePiece(piece, from, to, promotion);

        SwitchSideToMove();
        UpdateGameState();

        string moveNotation = BuildMoveNotation(piece, targetPiece, from, to, promotion);

        return new MoveDTO()
        {
            Number = fullMoveNumber,
            Side = piece.Side,
            Notation = moveNotation
        };
    }

    private void UpdateEnpassantSquare(PieceBase piece, Square to)
    {
        if (piece.Type == PieceType.PAWN)
        {
            var pos = board.GetSquareOfPiece(piece);
            if (piece.Side == Side.WHITE && pos.Rank == 6 || piece.Side == Side.BLACK && pos.Rank == 1)
                EnpassantSquare = to;
        }
    }

    private void UpdateCastlingRights(PieceBase piece, Square from)
    {
        if (SideToMove == Side.WHITE && (CastlingRightWhiteQueenSide || CastlingRightWhiteKingSide))
        {
            if (piece.Type == PieceType.KING)
            {
                CastlingRightWhiteQueenSide = false;
                CastlingRightWhiteKingSide = false;
            }
            else if (piece.Type == PieceType.ROOK)
            {
                if (from.Rank == 7 && from.File == 0)
                    CastlingRightWhiteQueenSide = false;
                else if (from.Rank == 7 && from.File == 7)
                    CastlingRightWhiteKingSide = false;
            }
        }
        else if (SideToMove == Side.BLACK && (CastlingRightBlackQueenSide || CastlingRightBlackKingSide))
        {
            if (piece.Type == PieceType.KING)
            {
                CastlingRightBlackQueenSide = false;
                CastlingRightBlackKingSide = false;
            }
            else if (piece.Type == PieceType.ROOK)
            {
                if (from.Rank == 0 && from.File == 0)
                    CastlingRightWhiteQueenSide = false;
                else if (from.Rank == 0 && from.File == 7)
                    CastlingRightWhiteKingSide = false;
            }
        }
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
                    throw new Exception("Invalid promotion type exception");

                var promotionPiece = new PieceFactory().BuildPiece(promotion, piece.Side);

                board.PromotePiece(from, to, promotionPiece);
            }
            else
                throw new Exception("Promotion expected exception!");
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
            var tempPiece = new PieceFactory().BuildPiece(promotion, piece.Side);

            notation += $"={tempPiece.Notation}";
        }

        if (board.KingIsMated(SideToMove))
            notation += "#";
        else if (board.KingInCheck(SideToMove))
            notation += "+";

        if (piece.Type == PieceType.PAWN && to.Equals(EnpassantSquare))
            notation += " e.p.";

        return notation;
    }

}


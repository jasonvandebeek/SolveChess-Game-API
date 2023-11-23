
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess;

public class Board
{

    private readonly PieceBase?[,] board;

    public bool CastlingRightBlackKingSide { get; set; }
    public bool CastlingRightBlackQueenSide { get; set; }
    public bool CastlingRightWhiteKingSide { get; set; }
    public bool CastlingRightWhiteQueenSide { get; set; }

    public Square? EnpassantSquare { get; set; }

    public string Fen { get { return BoardFenMapper.GetFenFromBoard(board); } }

    public Board(string fen, bool castlingRightBlackKingSide = false, bool castlingRightBlackQueenSide = false, bool castlingRightWhiteKingSide = false, bool castlingRightWhiteQueenSide = false, Square? enpassantSquare = null)
    {
        board = BoardFenMapper.GetBoardStateFromFen(fen);

        CastlingRightBlackKingSide = castlingRightBlackKingSide;
        CastlingRightBlackQueenSide = castlingRightBlackQueenSide;
        CastlingRightWhiteKingSide = castlingRightWhiteKingSide;
        CastlingRightWhiteQueenSide = castlingRightWhiteQueenSide;

        EnpassantSquare = enpassantSquare;
    }

    public Board(Board board)
    {
        this.board = board.GetBoardArray();

        CastlingRightBlackKingSide = board.CastlingRightBlackKingSide;
        CastlingRightBlackQueenSide = board.CastlingRightBlackQueenSide;
        CastlingRightWhiteKingSide = board.CastlingRightWhiteKingSide;
        CastlingRightWhiteQueenSide = board.CastlingRightWhiteQueenSide;

        EnpassantSquare = board.EnpassantSquare;
    }

    public Square GetSquareOfPiece(PieceBase piece)
    {
        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                if (piece.Equals(board[rank, file]))
                {
                    return new Square(rank, file);
                }
            }
        }

        throw new InvalidOperationException("Invalid board given with piece!");
    }

    public PieceBase? GetPieceAt(Square square)
    {
        return board[square.Rank, square.File];
    }

    public PieceBase?[,] GetBoardArray()
    {
        return board;
    }

    public void MovePiece(Square from, Square to)
    {
        PieceBase? piece = board[from.Rank, from.File];
        if (piece == null)
            return;

        board[from.Rank, from.File] = null;
        board[to.Rank, to.File] = piece;
    }

    public bool CanPieceMoveTo(PieceBase piece, Square target)
    {
        return piece.CanMoveToSquare(target, this);
    }

    public bool KingInCheck(Side side)
    {
        var king = GetKing(side);
        if (king == null)
            return false;

        return king.IsChecked(this);
    }

    public bool KingInDraw(Side side)
    {
        if(KingInCheck(side)) 
            return false;

        foreach (PieceBase? piece in board)
        {
            if (piece == null)
                continue;

            if (piece.GetPossibleMoves(this).Any())
                return false;
        }

        return true;
    }

    public bool KingIsMated(Side side)
    {
        if(!KingInCheck(side)) 
            return false;

        foreach(PieceBase? piece in board)
        {
            if(piece == null) 
                continue;

            if (piece.GetPossibleMoves(this).Any())
                return false;
        }

        return true;
    }

    public King? GetKing(Side side)
    {
        for(int rank = 0; rank < board.GetLength(0); rank++)
        {
            for(int file = 0; file < board.GetLength(1); file++)
            {
                PieceBase? piece = board[rank, file];

                if (piece == null || piece.Type != PieceType.KING || piece.Side != side)
                    continue;

                return (King) piece;
            }
        }

        return null;
    }

    public void PromotePiece(Square from, Square to, PieceBase promotionPiece)
    {
        board[from.Rank, from.File] = null;
        board[to.Rank, to.File] = promotionPiece;
    }

}


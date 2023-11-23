
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess;

public class Board
{

    private readonly PieceBase?[,] _boardArray;
    public PieceBase?[,] BoardArray 
    { 
        get 
        {
            PieceBase?[,] clonedArray = new PieceBase?[_boardArray.GetLength(0), _boardArray.GetLength(1)];

            for (int rank = 0; rank < _boardArray.GetLength(0); rank++)
                for (int file = 0; file < _boardArray.GetLength(1); file++)
                    clonedArray[rank, file] = _boardArray[rank, file];

            return clonedArray;
        } 
    }

    public bool CastlingRightBlackKingSide { get; set; }
    public bool CastlingRightBlackQueenSide { get; set; }
    public bool CastlingRightWhiteKingSide { get; set; }
    public bool CastlingRightWhiteQueenSide { get; set; }

    public Square? EnpassantSquare { get; set; }

    public string Fen { get { return BoardFenMapper.GetFenFromBoard(_boardArray); } }

    public Board(string fen, bool castlingRightBlackKingSide = false, bool castlingRightBlackQueenSide = false, bool castlingRightWhiteKingSide = false, bool castlingRightWhiteQueenSide = false, Square? enpassantSquare = null)
    {
        _boardArray = BoardFenMapper.GetBoardStateFromFen(fen);

        CastlingRightBlackKingSide = castlingRightBlackKingSide;
        CastlingRightBlackQueenSide = castlingRightBlackQueenSide;
        CastlingRightWhiteKingSide = castlingRightWhiteKingSide;
        CastlingRightWhiteQueenSide = castlingRightWhiteQueenSide;

        EnpassantSquare = enpassantSquare;
    }

    public Board(Board board)
    {
        _boardArray = board.BoardArray;

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
                if (piece.Equals(_boardArray[rank, file]))
                {
                    return new Square(rank, file);
                }
            }
        }

        throw new InvalidOperationException("Invalid board given with piece!");
    }

    public PieceBase? GetPieceAt(Square square)
    {
        return _boardArray[square.Rank, square.File];
    }

    public void MovePiece(Square from, Square to)
    {
        PieceBase? piece = _boardArray[from.Rank, from.File];
        if (piece == null)
            return;

        _boardArray[from.Rank, from.File] = null;
        _boardArray[to.Rank, to.File] = piece;
    }

    public void PromotePiece(Square from, Square to, PieceBase promotionPiece)
    {
        _boardArray[from.Rank, from.File] = null;
        _boardArray[to.Rank, to.File] = promotionPiece;
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

        foreach (PieceBase? piece in _boardArray)
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

        foreach(PieceBase? piece in _boardArray)
        {
            if(piece == null) 
                continue;

            if (piece.GetPossibleMoves(this).Any())
                return false;
        }

        return true;
    }

    private King? GetKing(Side side)
    {
        for(int rank = 0; rank < _boardArray.GetLength(0); rank++)
        {
            for(int file = 0; file < _boardArray.GetLength(1); file++)
            {
                PieceBase? piece = _boardArray[rank, file];

                if (piece == null || piece.Type != PieceType.KING || piece.Side != side)
                    continue;

                return (King) piece;
            }
        }

        return null;
    }

}


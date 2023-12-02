
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Factories;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;
using System.Drawing;

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

    public Board(bool castlingRightBlackKingSide = false, bool castlingRightBlackQueenSide = false, bool castlingRightWhiteKingSide = false, bool castlingRightWhiteQueenSide = false, Square? enpassantSquare = null)
    {
        _boardArray = new PieceBase?[8,8];
        
        CastlingRightBlackKingSide = castlingRightBlackKingSide;
        CastlingRightBlackQueenSide = castlingRightBlackQueenSide;
        CastlingRightWhiteKingSide = castlingRightWhiteKingSide;
        CastlingRightWhiteQueenSide = castlingRightWhiteQueenSide;

        EnpassantSquare = enpassantSquare;
    }

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

    public void PlacePieceAtSquare(PieceBase piece, Square square)
    {
        _boardArray[square.Rank, square.File] = piece;
    }

    public Square GetSquareOfPiece(PieceBase piece)
    {
        for (int rank = 0; rank < 8; rank++)
        {
            Square? square = FindPieceOnRank(rank, piece);
            if(square != null)
                return square;
        }

        throw new InvalidOperationException("Invalid board given with piece!");
    }

    public PieceBase? GetPieceAt(Square square)
    {
        return _boardArray[square.Rank, square.File];
    }

    public void MovePiece(Square from, Square to)
    {
        PieceBase? piece = GetPieceAt(from);
        if (piece == null)
            return;

        _boardArray[from.Rank, from.File] = null;
        _boardArray[to.Rank, to.File] = piece;
    }

    public void PromotePiece(Square from, Square to, PieceType promotionType)
    {
        PieceBase? piece = GetPieceAt(from);
        if (piece == null)
            return;

        var promotionPiece = PieceFactory.BuildPiece(promotionType, piece.Side);

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

        return !AnyPieceOfSideHasMoves(side);
    }

    public bool KingIsMated(Side side)
    {
        if(!KingInCheck(side)) 
            return false;

        return !AnyPieceOfSideHasMoves(side);
    }


    private bool IsPieceAtPosition(PieceBase piece, int rank, int file) 
    {
        return piece.Equals(_boardArray[rank, file]);
    }   

    private Square? FindPieceOnRank(int rank, PieceBase piece)
    {
        for (int file = 0; file < _boardArray.GetLength(1); file++)
        {
            if (IsPieceAtPosition(piece, rank, file))
                return new Square(rank, file);
        }

        return null;
    }

    private King? GetKing(Side side)
    {
        for(int rank = 0; rank < _boardArray.GetLength(0); rank++)
        {
            King? king = FindKingOnRank(side, rank);
            if(king != null)
                return king;
        }

        return null;
    }

    private bool AnyPieceOfSideHasMoves(Side side)
    {
        foreach (PieceBase? piece in _boardArray)
        {
            if (piece != null && piece.Side == side && piece.HasPossibleMoves(this))
                return true;
        }

        return false;
    }

    private King? FindKingOnRank(Side side, int rank)
    {
        for (int file = 0; file < _boardArray.GetLength(1); file++)
        {
            if (PositionHasKingOfSide(side, rank, file))
                return GetPieceAt(new Square(rank, file)) as King;
        }

        return null;
    }

    private bool PositionHasKingOfSide(Side side, int rank, int file)
    {
        PieceBase? piece = _boardArray[rank, file];

        return piece != null && piece.Type == PieceType.KING && piece.Side == side;
    }


}


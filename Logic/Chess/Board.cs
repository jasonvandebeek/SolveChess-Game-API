
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Factories;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;
using System.Text;
using System.Text.RegularExpressions;

namespace SolveChess.Logic.Chess;

public class Board
{

    private readonly PieceBase?[,] board = new PieceBase?[8, 8];

    public bool CastlingRightBlackKingSide { get; set; }
    public bool CastlingRightBlackQueenSide { get; set; }
    public bool CastlingRightWhiteKingSide { get; set; }
    public bool CastlingRightWhiteQueenSide { get; set; }

    public Square? EnpassantSquare { get; set; }

    public string Fen {
        get
        {
            var fen = new StringBuilder();

            for (int rank = 0; rank < board.GetLength(0); rank++)
            {
                int empty = 0;
                for(int file = 0; file < board.GetLength(1); file++)
                {
                    PieceBase? piece = board[rank, file];
                    if(piece == null)
                    {
                        empty += 1;
                        continue;
                    }

                    if (empty > 0)
                        fen.Append(empty);

                    fen.Append(piece.Notation);
                }

                if (empty > 0)
                    fen.Append(empty);

                fen.Append('/');
            }

            fen.Remove(fen.Length - 1, 1);
            return fen.ToString();
        }
    }

    public Board(string fen, bool castlingRightBlackKingSide = false, bool castlingRightBlackQueenSide = false, bool castlingRightWhiteKingSide = false, bool castlingRightWhiteQueenSide = false, Square? enpassantSquare = null)
    {
        if (!IsValidFEN(fen))
            throw new InvalidOperationException("Invalid FEN!");

        string[] ranks = fen.Split(' ')[0].Split('/');

        for (int rank = 0; rank < 8; rank++)
        {
            int file = 0;
            foreach (char fenChar in ranks[rank])
            {
                if (char.IsDigit(fenChar))
                {
                    file += int.Parse(fenChar.ToString());
                }
                else
                {
                    Side side = char.IsUpper(fenChar) ? Side.WHITE : Side.BLACK;

                    board[rank, file] = PieceFactory.BuildPiece(fenChar, side);
                    file++;
                }
            }
        }

        CastlingRightBlackKingSide = castlingRightBlackKingSide;
        CastlingRightBlackQueenSide = castlingRightBlackQueenSide;
        CastlingRightWhiteKingSide = castlingRightWhiteKingSide;
        CastlingRightWhiteQueenSide = castlingRightWhiteQueenSide;

        EnpassantSquare = enpassantSquare;
    }

    public Board(Board board)
    {
        PieceBase?[,] originalBoard = board.GetBoardArray();
        this.board = new PieceBase?[originalBoard.GetLength(0), originalBoard.GetLength(1)];

        for (int i = 0; i < originalBoard.GetLength(0); i++)
        {
            for (int j = 0; j < originalBoard.GetLength(1); j++)
            {
                this.board[i, j] = originalBoard[i, j];
            }
        }

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

    static private bool IsValidFEN(string fen)
    {
        string pattern = @"^\s*(((?:[rnbqkpRNBQKP1-8]+\/){7})[rnbqkpRNBQKP1-8]+)(\s([b|w]))?(\s([K|Q|k|q]{1,4}))?(\s(-|[a-h][1-8]))?(\s(\d+\s\d+))?$";

        try
        {
            var regex = new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(1));
            Match match = regex.Match(fen);

            return match.Success;
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

}


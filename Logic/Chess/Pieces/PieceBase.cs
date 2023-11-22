
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;
using System.Linq;

namespace SolveChess.Logic.Chess.Pieces;

public abstract class PieceBase
{

    private readonly Side _side;
    public Side Side { get { return _side; } }

    public abstract PieceType Type { get; }

    protected abstract char _notation { get; }
    public char Notation
    {
        get
        {
            return _side == Side.WHITE ? char.ToUpper(_notation) : char.ToLower(_notation);
        }
    }

    protected PieceBase(Side side)
    {
        _side = side;
    }

    public abstract IEnumerable<Square> GetPossibleMoves(Board board);

    public bool CanMoveToSquare(Square target, Board board)
    {
        return GetPossibleMoves(board).Any(move => target.Equals(move));
    }

    protected IEnumerable<Square> PawnMoves(Board board)
    {
        Square startingPosition = board.GetSquareOfPiece(this);
        int forwardDirection = _side == Side.WHITE ? -1 : 1;
        var moves = new List<Square>();

        void AddMove(int rankOffset, int fileOffset)
        {
            var targetSquare = new Square(startingPosition.Rank + rankOffset, startingPosition.File + fileOffset);
            if (board.GetPieceAt(targetSquare) == null)
                moves.Add(targetSquare);
        }

        AddMove(forwardDirection, 0);

        if (_side == Side.WHITE && startingPosition.Rank == 6 || _side == Side.BLACK && startingPosition.Rank == 1)
            AddMove(forwardDirection * 2, 0);

        foreach (int fileOffset in new[] { -1, 1 })
        {
            int newFile = startingPosition.File + fileOffset;
            if (newFile > 7 || newFile < 0)
                continue;

            var targetSquare = new Square(startingPosition.Rank + forwardDirection, newFile);
            PieceBase? target = board.GetPieceAt(targetSquare);
            if ((target != null && target.Side != _side) || targetSquare.Equals(board.EnpassantSquare))
                moves.Add(targetSquare);
        }

        return moves;
    }

    protected IEnumerable<Square> RookMoves(Board board)
    {
        PieceBase?[,] boardArray = board.GetBoardArray();
        Square startingPosition = board.GetSquareOfPiece(this);
        var moves = new List<Square>();

        for (int i = 0; i < 4; i++)
        {
            int rankOffset = new int[] { 1, 0, -1, 0 }[i];
            int fileOffset = new int[] { 0, -1, 0, 1 }[i];

            int rank = startingPosition.Rank + rankOffset;
            int file = startingPosition.File + fileOffset;

            while (IsWithinBoardBounds(rank, file))
            {
                PieceBase? target = boardArray[rank, file];

                if (target != null)
                {
                    if (target.Side != _side)
                        moves.Add(new Square(rank, file));

                    break;
                }

                moves.Add(new Square(rank, file));

                rank += rankOffset;
                file += fileOffset;
            }
        }

        return moves;
    }

    protected IEnumerable<Square> KnightMoves(Board board)
    {
        PieceBase?[,] boardArray = board.GetBoardArray();
        Square startingPosition = board.GetSquareOfPiece(this);
        var moves = new List<Square>();

        for (int i = 0; i < 8; i++)
        {
            int rankOffset = new int[] { 1, 2, 2, 1, -1, -2, -2, -1 }[i];
            int fileOffset = new int[] { 2, 1, -1, -2, -2, -1, 1, 2 }[i];

            int rank = startingPosition.Rank + rankOffset;
            int file = startingPosition.File + fileOffset;

            if (!IsWithinBoardBounds(rank, file))
                continue;

            PieceBase? target = boardArray[rank, file];

            if (target != null && target.Side == _side)
                continue;

            moves.Add(new Square(rank, file));
        }

        return moves;
    }

    protected IEnumerable<Square> BishopMoves(Board board)
    {
        PieceBase?[,] boardArray = board.GetBoardArray();
        Square startingPosition = board.GetSquareOfPiece(this);
        var moves = new List<Square>();

        for (int i = 0; i < 4; i++)
        {
            int rankOffset = new int[] { 1, 1, -1, -1 }[i];
            int fileOffset = new int[] { 1, -1, 1, -1 }[i];

            int rank = startingPosition.Rank + rankOffset;
            int file = startingPosition.File + fileOffset;

            while (IsWithinBoardBounds(rank, file))
            {
                PieceBase? target = boardArray[rank, file];

                if (target != null)
                {
                    if (target.Side == _side)
                        break;


                    moves.Add(new Square(rank, file));
                    break;
                }

                moves.Add(new Square(rank, file));

                rank += rankOffset;
                file += fileOffset;
            }
        }

        return moves;
    }

    protected IEnumerable<Square> QueenMoves(Board board)
    {
        return RookMoves(board).Concat(BishopMoves(board));
    }

    protected IEnumerable<Square> KingMoves(Board board)
    {
        PieceBase?[,] boardArray = board.GetBoardArray();
        Square startingPosition = board.GetSquareOfPiece(this);
        var moves = new List<Square>();

        for (int i = 0; i < 8; i++)
        {
            int rankOffset = new int[] { 1, 1, 1, 0, 0, -1, -1, -1 }[i];
            int fileOffset = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 }[i];

            int rank = startingPosition.Rank + rankOffset;
            int file = startingPosition.File + fileOffset;

            if (!IsWithinBoardBounds(rank, file))
                continue;

            PieceBase? target = boardArray[rank, file];

            if (target != null && target.Side == _side)
                continue;

            moves.Add(new Square(rank, file));
        }

        return moves.Concat(KingCastlingMoves(board));
    }

    private IEnumerable<Square> KingCastlingMoves(Board board)
    {
        if (Side == Side.WHITE)
        {
            if (board.CastlingRightWhiteKingSide)
                yield return new Square(7, 6);

            if (board.CastlingRightWhiteQueenSide)
                yield return new Square(7, 1);
        }
        else
        {
            if (board.CastlingRightBlackKingSide)
                yield return new Square(0, 6);

            if (board.CastlingRightBlackQueenSide)
                yield return new Square(0, 1);
        }
    }

    protected IEnumerable<Square> FilterOutIllegalMoves(IEnumerable<Square> moves, Board board)
    {
        var legalMoves = new List<Square>();

        foreach(var move in moves)
        {
            var clone = new Board(board);
            Square currentSquare = board.GetSquareOfPiece(this);

            clone.MovePiece(currentSquare, move);
            if (!clone.KingInCheck(_side))
                legalMoves.Add(move);
        }

        return legalMoves;
    }

    private static bool IsWithinBoardBounds(int rank, int file)
    {
        return rank >= 0 && rank < 8 && file >= 0 && file < 8;
    }

}


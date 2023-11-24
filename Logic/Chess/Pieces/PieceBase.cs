
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
        var moves = new List<Square>();
        Square startingPosition = board.GetSquareOfPiece(this);
        int forwardDirection = _side == Side.WHITE ? -1 : 1;
        
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
        var rankOffsets = new int[] { 1, 0, -1, 0 };
        var fileOffsets = new int[] { 0, -1, 0, 1 };

        return MultiDirectionalTraceMoves(board, rankOffsets, fileOffsets);
    }

    protected IEnumerable<Square> KnightMoves(Board board)
    {
        var rankOffsets = new int[] { 1, 2, 2, 1, -1, -2, -2, -1 };
        var fileOffsets = new int[] { 2, 1, -1, -2, -2, -1, 1, 2 };

        return MultiPointSingularMoves(board, rankOffsets, fileOffsets);
    }

    protected IEnumerable<Square> BishopMoves(Board board)
    {
        var rankOffsets = new int[] { 1, 1, -1, -1 };
        var fileOffsets = new int[] { 1, -1, 1, -1 };

        return MultiDirectionalTraceMoves(board, rankOffsets, fileOffsets);
    }

    protected IEnumerable<Square> QueenMoves(Board board)
    {
        return RookMoves(board).Concat(BishopMoves(board));
    }

    protected IEnumerable<Square> KingMoves(Board board)
    {
        var rankOffsets = new int[] { 1, 1, 1, 0, 0, -1, -1, -1 };
        var fileOffsets = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };

        var moves = MultiPointSingularMoves(board, rankOffsets, fileOffsets).ToList();
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
    
    private IEnumerable<Square> MultiPointSingularMoves(Board board, int[] rankOffsets, int[] fileOffsets)
    {
        if (rankOffsets.Length != fileOffsets.Length)
            yield break;

        for (int i = 0; i < rankOffsets.Length; i++)
        {
            var move = SingularMove(board, rankOffsets[i], fileOffsets[i]);
            if (move == null)
                continue;

            yield return move;
        }
    }

    private IEnumerable<Square> MultiDirectionalTraceMoves(Board board, int[] rankOffsets, int[] fileOffsets)
    {
        if(rankOffsets.Length != fileOffsets.Length)
            return Enumerable.Empty<Square>();

        var moves = new List<Square>();

        for (int i = 0; i < rankOffsets.Length; i++)
        {
            moves = moves.Concat(TraceMoves(board, rankOffsets[i], fileOffsets[i])).ToList();
        }

        return moves;
    }

    private IEnumerable<Square> TraceMoves(Board board, int rankOffset, int fileOffset)
    {
        Square startingPosition = board.GetSquareOfPiece(this);

        int rank = startingPosition.Rank + rankOffset;
        int file = startingPosition.File + fileOffset;

        bool keepLooking = true;
        while (IsWithinBoardBounds(rank, file) && keepLooking)
        {
            var targetSquare = new Square(rank, file);
            PieceBase? targetPiece = board.GetPieceAt(targetSquare);

            if (targetPiece != null)
            {
                if (targetPiece.Side != _side)
                    yield return targetSquare;

                keepLooking = false;
            }
            else
            {
                rank += rankOffset;
                file += fileOffset;

                yield return targetSquare;
            }
        }
    }

    private Square? SingularMove(Board board, int rankOffset, int fileOffset)
    {
        Square startingPosition = board.GetSquareOfPiece(this);

        int rank = startingPosition.Rank + rankOffset;
        int file = startingPosition.File + fileOffset;

        if (!IsWithinBoardBounds(rank, file))
            return null;

        var targetSquare = new Square(rank, file);
        PieceBase? targetPiece = board.GetPieceAt(targetSquare);
        if (targetPiece != null && targetPiece.Side == _side)
            return null;

        return targetSquare;

    }

    protected IEnumerable<Square> FilterOutIllegalMoves(IEnumerable<Square> moves, Board board)
    {
        var legalMoves = new List<Square>();

        foreach(var move in moves)
        {
            var clone = new Board(board);
            Square currentSquare = clone.GetSquareOfPiece(this);

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



using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;
using System.Linq;

namespace SolveChess.Logic.Chess.Pieces;

public abstract class PieceBase
{

    private readonly Side _side;
    public Side Side { get { return _side; } }

    public abstract PieceType Type { get; }

    private readonly char _notation;
    public char Notation
    {
        get
        {
            return _side == Side.WHITE ? char.ToUpper(_notation) : char.ToLower(_notation);
        }
    }

    protected PieceBase(Side side, char notation)
    {
        _side = side;
        _notation = notation;
    }

    public abstract IEnumerable<Square> GetPossibleMoves(Board board);

    public bool CanMoveToSquare(Square target, Board board)
    {
        return GetPossibleMoves(board).Any(move => target.Equals(move));
    }

    protected IEnumerable<Square> PawnMoves(Board board)
    {
        return GetForwardPawnMoves(board).Concat(GetAttackingPawnMoves(board));
    }

    private IEnumerable<Square> GetForwardPawnMoves(Board board)
    {
        var moveOne = GetForwardPawnMove(1, board);
        if(moveOne == null)
            yield break;

        yield return moveOne;

        var moveTwo = GetForwardPawnMove(2, board);
        if(PawnIsAtStartingPosition(board) && moveTwo != null)
            yield return moveTwo;
    }

    private bool PawnIsAtStartingPosition(Board board)
    {
        Square position = board.GetSquareOfPiece(this);
        return position.Rank == 1 || position.Rank == 6;
    }

    private Square? GetForwardPawnMove(int rankOffsetMutliplier, Board board)
    {
        Square startingPosition = board.GetSquareOfPiece(this);
        int forwardDirection = _side == Side.WHITE ? -1 : 1;

        if (!IsWithinBoardBounds(startingPosition.Rank + (rankOffsetMutliplier * forwardDirection), startingPosition.File))
            return null;

        var targetSquare = new Square(startingPosition.Rank + (rankOffsetMutliplier * forwardDirection), startingPosition.File);
        if (board.GetPieceAt(targetSquare) != null)
            return null;

        return targetSquare;
    }

    private IEnumerable<Square> GetAttackingPawnMoves(Board board)
    {
        foreach(int offset in new[] { -1, 1 }) {
            var move = GetAttackingPawnMove(offset, board);

            if (move != null) 
                yield return move;
        }
    }

    private Square? GetAttackingPawnMove(int fileOffset, Board board)
    {
        Square startingPosition = board.GetSquareOfPiece(this);
        int forwardDirection = _side == Side.WHITE ? -1 : 1;

        if (!IsWithinBoardBounds(startingPosition.Rank + forwardDirection, startingPosition.File + fileOffset))
            return null;

        var targetSquare = new Square(startingPosition.Rank + forwardDirection, startingPosition.File + fileOffset);
        if (!PawnIsAttackingSquare(targetSquare, board))
            return null;

        return targetSquare;
    }

    private bool PawnIsAttackingSquare(Square targetSquare, Board board)
    {
        PieceBase? target = board.GetPieceAt(targetSquare);

        return (target != null && target.Side != _side) || targetSquare.Equals(board.EnpassantSquare);
    }

    protected IEnumerable<Square> RookMoves(Board board)
    {
        var rankOffsets = new[] { 1, 0, -1, 0 };
        var fileOffsets = new[] { 0, -1, 0, 1 };

        return MultiDirectionalTraceMoves(board, rankOffsets, fileOffsets);
    }

    protected IEnumerable<Square> KnightMoves(Board board)
    {
        var rankOffsets = new[] { 1, 2, 2, 1, -1, -2, -2, -1 };
        var fileOffsets = new[] { 2, 1, -1, -2, -2, -1, 1, 2 };

        return MultiPointSingularMoves(board, rankOffsets, fileOffsets);
    }

    protected IEnumerable<Square> BishopMoves(Board board)
    {
        var rankOffsets = new[] { 1, 1, -1, -1 };
        var fileOffsets = new[] { 1, -1, 1, -1 };

        return MultiDirectionalTraceMoves(board, rankOffsets, fileOffsets);
    }

    protected IEnumerable<Square> QueenMoves(Board board)
    {
        return RookMoves(board).Concat(BishopMoves(board));
    }

    protected IEnumerable<Square> KingMoves(Board board)
    {
        var rankOffsets = new[] { 1, 1, 1, 0, 0, -1, -1, -1 };
        var fileOffsets = new[] { -1, 0, 1, -1, 1, -1, 0, 1 };

        return MultiPointSingularMoves(board, rankOffsets, fileOffsets);
    }

    protected IEnumerable<Square> MultiPointSingularMoves(Board board, int[] rankOffsets, int[] fileOffsets)
    {
        for (int i = 0; i < rankOffsets.Length; i++)
        {
            var move = SingularMove(board, rankOffsets[i], fileOffsets[i]);
            if (move != null)
                yield return move;
        }
    }

    protected IEnumerable<Square> MultiDirectionalTraceMoves(Board board, int[] rankOffsets, int[] fileOffsets)
    {
        var moves = new List<Square>();

        for (int i = 0; i < rankOffsets.Length; i++)
        {
            moves = moves.Concat(TraceMoves(board, rankOffsets[i], fileOffsets[i])).ToList();
        }

        return moves;
    }

    protected IEnumerable<Square> TraceMoves(Board board, int rankOffset, int fileOffset)
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

    protected Square? SingularMove(Board board, int rankOffset, int fileOffset)
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
        foreach(var move in moves)
        {
            if (!KingIsInCheckAfterMove(move, board))
                yield return move;
        }
    }

    private bool KingIsInCheckAfterMove(Square move, Board board)
    {
        var clone = new Board(board);
        Square currentSquare = clone.GetSquareOfPiece(this);

        clone.MovePiece(currentSquare, move);
        return clone.KingInCheck(_side);
    }

    private static bool IsWithinBoardBounds(int rank, int file)
    {
        return rank >= 0 && rank < 8 && file >= 0 && file < 8;
    }

    public bool HasPossibleMoves(Board board)
    {
        return GetPossibleMoves(board).Any();
    }

}


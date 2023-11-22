
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

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
        PieceBase?[,] boardArray = board.GetBoardArray();
        Square startingPosition = board.GetSquareOfPiece(this);
        int forwardDirection = _side == Side.WHITE ? -1 : 1;
        var moves = new List<Square>();

        var targetSquare = new Square(startingPosition.Rank + forwardDirection, startingPosition.File);
        if (boardArray[targetSquare.Rank, targetSquare.File] == null)
        {
            moves.Add(targetSquare);

            if(_side == Side.WHITE && startingPosition.Rank == 6 || _side == Side.BLACK && startingPosition.Rank == 1) 
            {
                targetSquare = new Square(startingPosition.Rank + forwardDirection * 2, startingPosition.File);

                if (boardArray[targetSquare.Rank, targetSquare.File] == null)
                    moves.Add(targetSquare);
            }
        }

        if(startingPosition.File - 1 > 0)
        {
            targetSquare = new Square(startingPosition.Rank + forwardDirection, startingPosition.File - 1);
            PieceBase? target = boardArray[targetSquare.Rank, targetSquare.File];

            if (target != null && target.Side != _side || targetSquare.Equals(board.EnpassantSquare))
                moves.Add(targetSquare);
        }

        if (startingPosition.File + 1 < 8)
        {
            targetSquare = new Square(startingPosition.Rank + forwardDirection, startingPosition.File + 1);
            PieceBase? target = boardArray[targetSquare.Rank, targetSquare.File];

            if (target != null && target.Side != _side || targetSquare.Equals(board.EnpassantSquare))
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

        if (Side == Side.WHITE)
        {
            if (board.CastlingRightWhiteKingSide)
                moves.Add(new Square(7, 6));

            if (board.CastlingRightWhiteQueenSide)
                moves.Add(new Square(7, 1));
        }
        else
        {
            if (board.CastlingRightBlackKingSide)
                moves.Add(new Square(0, 6));

            if(board.CastlingRightBlackQueenSide)
                moves.Add(new Square(0, 1));
        }

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

        return moves;
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


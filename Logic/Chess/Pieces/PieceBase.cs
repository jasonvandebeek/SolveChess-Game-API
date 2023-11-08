﻿
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;
using System.Linq;

namespace SolveChess.Logic.Chess.Pieces;

public abstract class PieceBase
{

    private readonly Side _side;
    public Side Side { get; }

    public abstract PieceType Type { get; }

    protected abstract char _notation { get; }
    public char Notation
    {
        get
        {
            return _side == Side.WHITE ? char.ToUpper(_notation) : char.ToLower(_notation);
        }
    }

    public PieceBase(Side side)
    {
        _side = side;
    }

    public abstract IEnumerable<Square> GetPossibleMoves(Board board);

    public bool CanMoveToSquare(Square target, Board board)
    {
        IEnumerable<Square> moves = GetPossibleMoves(board);

        foreach(Square move in moves)
        {
            if (target.Equals(move))
                return true;
        }

        return false;
    }

    //TODO: add enpassant
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

            if (target != null && target.Side != _side)
                moves.Add(targetSquare);
        }

        if (startingPosition.File + 1 < 8)
        {
            targetSquare = new Square(startingPosition.Rank + forwardDirection, startingPosition.File + 1);
            PieceBase? target = boardArray[targetSquare.Rank, targetSquare.File];

            if (target != null && target.Side != this.Side)
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

            while (rank >= 0 && rank < 8 && file >= 0 && file < 8)
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

            if (rank >= 0 && rank < 8 && file >= 0 && file < 8)
            {
                PieceBase? target = boardArray[rank, file];

                if (target != null && target.Side == _side)
                    continue;

                moves.Add(new Square(rank, file));
            }
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

            while (rank >= 0 && rank < 8 && file >= 0 && file < 8)
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

    //Add castling
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

            if (rank >= 0 && rank < 8 && file >= 0 && file < 8)
            {
                PieceBase? target = boardArray[rank, file];

                if (target != null && target.Side == _side)
                    continue;

                moves.Add(new Square(rank, file));
            }
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

}


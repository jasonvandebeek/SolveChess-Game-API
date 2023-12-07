
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces;

public class King : PieceBase
{

    public override PieceType Type { get; } = PieceType.KING;

    public King(Side side) : base(side, 'k')
    {
    }

    public override IEnumerable<Square> GetPossibleMoves(Board board)
    {
        var moves = KingMoves(board).Concat(CastlingMoves(board));

        return FilterOutIllegalMoves(moves, board);
    }

    private IEnumerable<Square> CastlingMoves(Board board)
    {
        if (Side == Side.WHITE)
        {
            return WhiteSideCastlingMoves(board);
        }
        else
        {
            return BlackSideCastlingMoves(board);
        }
    }

    private IEnumerable<Square> WhiteSideCastlingMoves(Board board)
    {
        if (!IsAtStartingPosition(board))
            yield break;

        if (board.CastlingRightWhiteKingSide && KingSideClear(board))
            yield return new Square(7, 6);

        if (board.CastlingRightWhiteQueenSide && QueenSideClear(board))
            yield return new Square(7, 2);
    }

    private bool IsAtStartingPosition(Board board)
    {
        var square = board.GetSquareOfPiece(this);

        return (square.Equals(new Square("e1")) && Side == Side.WHITE) || (square.Equals(new Square("e8")) && Side == Side.BLACK);
    }

    private IEnumerable<Square> BlackSideCastlingMoves(Board board)
    {
        if (!IsAtStartingPosition(board))
            yield break;

        if (board.CastlingRightBlackKingSide && KingSideClear(board))
            yield return new Square(0, 6);

        if (board.CastlingRightBlackQueenSide && QueenSideClear(board))
            yield return new Square(0, 2);
    }

    private bool QueenSideClear(Board board)
    {
        var moves = TraceMoves(board, 0, -1);
        return moves.Count() == 3;
    }

    private bool KingSideClear(Board board)
    {
        var moves = TraceMoves(board, 0, 1);
        return moves.Count() == 2;
    }

    public bool IsChecked(Board board) 
    {
        if (AttackingPieceOfType(board, PawnMoves(board), PieceType.PAWN))
            return true;

        if (AttackingPieceOfType(board, RookMoves(board), PieceType.ROOK))
            return true;

        if (AttackingPieceOfType(board, KnightMoves(board), PieceType.KNIGHT))
            return true;

        if (AttackingPieceOfType(board, BishopMoves(board), PieceType.BISHOP))
            return true;

        if (AttackingPieceOfType(board, QueenMoves(board), PieceType.QUEEN))
            return true;

        if (AttackingPieceOfType(board, KingMoves(board), PieceType.KING))
            return true;

        return false;
    }
    
    private static bool AttackingPieceOfType(Board board, IEnumerable<Square> moves, PieceType type)
    {
        foreach(var move in moves)
        {
            PieceBase? target = board.GetPieceAt(move);
            if(target != null && target.Type == type) 
            {
                return true;
            }
        }

        return false;
    }

}


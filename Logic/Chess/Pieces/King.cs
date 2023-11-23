
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces;

public class King : PieceBase
{

    public override PieceType Type { get; } = PieceType.KING;

    protected override char _notation { get; } = 'k';

    public King(Side side) : base(side)
    {
    }

    public override IEnumerable<Square> GetPossibleMoves(Board board)
    {
        return FilterOutIllegalMoves(KingMoves(board), board);
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



using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces;

public class Pawn : PieceBase
{
    
    public override PieceType Type { get; } = PieceType.PAWN;

    protected override char _notation { get; } = 'p';

    public Pawn(Side side) : base(side)
    {
    }

    public override IEnumerable<Square> GetPossibleMoves(Board board)
    {
        return FilterOutIllegalMoves(PawnMoves(board), board);
    }

}


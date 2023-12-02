
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces;

public class Pawn : PieceBase
{
    
    public override PieceType Type { get; } = PieceType.PAWN;

    public Pawn(Side side) : base(side, 'p')
    {
    }

    public override IEnumerable<Square> GetPossibleMoves(Board board)
    {
        return FilterOutIllegalMoves(PawnMoves(board), board);
    }

}


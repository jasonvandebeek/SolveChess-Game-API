
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces;

public class Bishop : PieceBase
{

    public override PieceType Type { get; } = PieceType.BISHOP;

    public Bishop(Side side) : base(side, 'b')
    {
    }

    public override IEnumerable<Square> GetPossibleMoves(Board board)
    {
        return FilterOutIllegalMoves(BishopMoves(board), board);
    }

}


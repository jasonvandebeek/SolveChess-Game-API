
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces;

public class Bishop : PieceBase
{

    public override PieceType Type { get; } = PieceType.BISHOP;

    protected override char _notation { get; } = 'b';

    public Bishop(Side side) : base(side)
    {
    }

    public override IEnumerable<Square> GetPossibleMoves(Board board)
    {
        return FilterOutIllegalMoves(BishopMoves(board), board);
    }

}


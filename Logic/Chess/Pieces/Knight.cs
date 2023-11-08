
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces;

public class Knight : PieceBase
{

    public override PieceType Type { get; } = PieceType.KNIGHT;

    protected override char _notation { get; } = 'n';

    public Knight(Side side) : base(side)
    {
    }

    public override IEnumerable<Square> GetPossibleMoves(Board board)
    {
        return FilterOutIllegalMoves(KnightMoves(board), board);
    }

}


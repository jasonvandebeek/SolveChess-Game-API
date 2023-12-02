
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces;

public class Knight : PieceBase
{

    public override PieceType Type { get; } = PieceType.KNIGHT;

    public Knight(Side side) : base(side, 'n')
    {
    }

    public override IEnumerable<Square> GetPossibleMoves(Board board)
    {
        return FilterOutIllegalMoves(KnightMoves(board), board);
    }

}


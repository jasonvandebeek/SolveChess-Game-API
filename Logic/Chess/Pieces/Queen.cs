
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces;

public class Queen : PieceBase
{

    public override PieceType Type { get; } = PieceType.QUEEN;

    public Queen(Side side) : base(side, 'q')
    {
    }

    public override IEnumerable<Square> GetPossibleMoves(Board board)
    {
        return FilterOutIllegalMoves(QueenMoves(board), board);
    }

}


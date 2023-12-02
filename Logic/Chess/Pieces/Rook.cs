
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces;

public class Rook : PieceBase
{

    public override PieceType Type { get; } = PieceType.ROOK;

    public Rook(Side side) : base(side, 'r')
    {
    }

    public override IEnumerable<Square> GetPossibleMoves(Board board)
    {
        return FilterOutIllegalMoves(RookMoves(board), board);
    }

}


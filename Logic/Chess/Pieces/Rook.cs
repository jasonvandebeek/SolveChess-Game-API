
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces;

public class Rook : PieceBase
{

    public override PieceType Type { get; } = PieceType.ROOK;

    protected override char _notation { get; } = 'r';

    public Rook(Side side) : base(side)
    {
    }

    public override IEnumerable<Square> GetPossibleMoves(Board board)
    {
        return FilterOutIllegalMoves(RookMoves(board), board);
    }

}


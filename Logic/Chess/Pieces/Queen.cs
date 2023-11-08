
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Chess.Pieces;

public class Queen : PieceBase
{

    public override PieceType Type { get; } = PieceType.QUEEN;

    protected override char _notation { get; } = 'q';

    public Queen(Side side) : base(side)
    {
    }

    public override IEnumerable<Square> GetPossibleMoves(Board board)
    {
        return FilterOutIllegalMoves(QueenMoves(board), board);
    }

}


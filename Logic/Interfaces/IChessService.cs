using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.DTO;

namespace SolveChess.Logic.Interfaces;

public interface IChessService
{

    bool UserHasAccessToGame(string gameId, string userId);
    void PlayMoveOnGame(string gameId, string userId, Square from, Square to, PieceType? promotion);
    IEnumerable<MoveDto> GetMoves(string gameId);

}

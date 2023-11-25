using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Interfaces;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.Models;
using SolveChess.Logic.ResultObjects;

namespace SolveChess.Logic.Interfaces;

public interface IChessService
{

    Task<bool> UserHasAccessToGame(string gameId, string userId);
    Task<MoveResult> PlayMoveOnGame(string gameId, string userId, ISquare from, ISquare to, string? promotion);
    Task<IEnumerable<Move>> GetPlayedMovesForGame(string gameId);
    Task CreateNewGame(string playerOneUserId, string playerTwoUserId, string? WhiteSideUserId);
    Task<GameInfoModel> GetGameWithId(string gameId);

}

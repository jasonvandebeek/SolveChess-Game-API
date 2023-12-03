
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.Models;

namespace SolveChess.Logic.DAL;

public interface IGameDal
{

    Task<GameInfoModel?> GetGameWithId(string gameId);
    Task<IEnumerable<Move>?> GetMovesForGame(string gameId);

    Task UpdateGameAndCreateMove(string gameId, Game game, Move move);
    Task CreateGame(GameInfoModel gameInfo);

}

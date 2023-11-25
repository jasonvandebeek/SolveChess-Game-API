
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.DTO;

namespace SolveChess.Logic.DAL;

public interface IGameDal
{

    Task<GameInfoModel> GetGameWithId(string gameId);
    Task<IEnumerable<Move>> GetMovesForGame(string gameId);

    Task UpdateGame(string gameId, Game game, Move move);

}

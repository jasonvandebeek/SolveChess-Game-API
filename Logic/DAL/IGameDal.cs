
using SolveChess.Logic.DTO;

namespace SolveChess.Logic.DAL;

public interface IGameDal
{

    public GameDto GetGame(string gameId);
    public IEnumerable<MoveDto> GetMoves(string gameId);

    public void UpdateGame(GameDto game);
    public void AddMove(string gameId, MoveDto move);

}

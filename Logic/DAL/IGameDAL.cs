
using SolveChess.Logic.DTO;

namespace SolveChess.Logic.DAL;

public interface IGameDAL
{

    public GameDTO GetGame(string gameId);
    public IEnumerable<MoveDTO> GetMoves(string gameId);

    public void UpdateGame(GameDTO game);
    public void AddMove(string gameId, MoveDTO move);

}

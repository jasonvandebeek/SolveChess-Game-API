
using Microsoft.EntityFrameworkCore;
using SolveChess.DAL.Model;
using SolveChess.Logic.DAL;
using SolveChess.Logic.DTO;

namespace SolveChess.DAL;

public class GameDAL : AppDbContext, IGameDAL
{

    public GameDAL(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public void AddMove(string gameId, MoveDTO move)
    {
        throw new NotImplementedException();
    }

    public GameDTO GetGame(string gameId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<MoveDTO> GetMoves(string gameId)
    {
        throw new NotImplementedException();
    }

    public void UpdateGame(GameDTO game)
    {
        throw new NotImplementedException();
    }

}


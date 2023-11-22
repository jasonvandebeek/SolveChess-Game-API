
using Microsoft.EntityFrameworkCore;
using SolveChess.DAL.Model;
using SolveChess.Logic.DAL;
using SolveChess.Logic.DTO;

namespace SolveChess.DAL;

public class GameDal : AppDbContext, IGameDal
{

    public GameDal(DbContextOptions<AppDbContext> options) : base(options)
    { 
    }

    public void AddMove(string gameId, MoveDto move)
    {
        throw new NotImplementedException();
    }

    public GameDto GetGame(string gameId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<MoveDto> GetMoves(string gameId)
    {
        throw new NotImplementedException();
    }

    public void UpdateGame(GameDto game)
    {
        throw new NotImplementedException();
    }

}


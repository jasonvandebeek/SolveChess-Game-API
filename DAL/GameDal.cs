
using Microsoft.EntityFrameworkCore;
using SolveChess.DAL.Model;
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.DAL;
using SolveChess.Logic.Models;

namespace SolveChess.DAL;

public class GameDal : AppDbContext, IGameDal
{

    public GameDal(DbContextOptions<AppDbContext> options) : base(options)
    { 
    }

    public Task<IEnumerable<Move>> GetMovesForGame(string gameId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateGame(string gameId, Game game, Move move)
    {
        throw new NotImplementedException();
    }

    public Task<GameInfoModel> GetGameWithId(string gameId)
    {
        throw new NotImplementedException();
    }

}


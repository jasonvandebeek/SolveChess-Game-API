
using Microsoft.EntityFrameworkCore;
using SolveChess.DAL.Exceptions;
using SolveChess.DAL.Model;
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.DAL;
using SolveChess.Logic.Models;

namespace SolveChess.DAL;

public class GameDal : IGameDal
{

    private readonly AppDbContext _dbContext;

    public GameDal(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Move>?> GetMovesForGame(string gameId)
    {
        var existingGame = await _dbContext.Game.FindAsync(gameId);
        if (existingGame == null)
            return null;

        var dbMoves = await _dbContext.Move
            .Where(m => m.GameId == gameId)
            .ToListAsync();

        var moves = new List<Move>();
        foreach(var dbMove in dbMoves)
        {
            var move = new Move(dbMove.Number, dbMove.Side, dbMove.Notation);

            moves.Add(move);
        }

        return moves;
    }

    public async Task UpdateGameAndCreateMove(string gameId, Game game, Move move)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await UpdateGame(gameId, game);
            await CreateMove(gameId, move);

            await transaction.CommitAsync();
        }
        catch(Exception exception)
        {
            await transaction.RollbackAsync();
            throw new DatabaseException("An error occured while updating game and creating move.", exception);
        }
    }

    private async Task UpdateGame(string gameId, Game game)
    {
        var gameModel = await _dbContext.Game.FindAsync(gameId) ?? throw new DatabaseException("There is no game with that id.");

        gameModel.State = game.State;
        gameModel.Fen = game.Fen;
        gameModel.CastlingRightBlackKingSide = game.CastlingRightBlackKingSide;
        gameModel.CastlingRightBlackQueenSide = game.CastlingRightBlackQueenSide;
        gameModel.CastlingRightWhiteKingSide = game.CastlingRightWhiteKingSide;
        gameModel.CastlingRightWhiteQueenSide = game.CastlingRightWhiteQueenSide;
        gameModel.FullMoveNumber = game.FullMoveNumber;
        gameModel.HalfMoveClock = game.HalfMoveClock;
        gameModel.SideToMove = game.SideToMove;

        await _dbContext.SaveChangesAsync();
    }

    private async Task CreateMove(string gameId, Move move)
    {
        var moveModel = new MoveModel()
        {
            GameId = gameId,
            Number = move.Number,
            Side = move.Side,
            Notation = move.Notation
        };

        _dbContext.Move.Add(moveModel);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<GameInfoModel?> GetGameWithId(string gameId)
    {
        var game = await _dbContext.Game.FindAsync(gameId);
        if (game == null)
            return null;

        var gameStateModel = new GameStateModel()
        {
            State = game.State,
            Fen = game.Fen,
            CastlingRightBlackKingSide = game.CastlingRightBlackKingSide,
            CastlingRightBlackQueenSide = game.CastlingRightBlackQueenSide,
            CastlingRightWhiteKingSide = game.CastlingRightWhiteKingSide,
            CastlingRightWhiteQueenSide = game.CastlingRightWhiteQueenSide,
            FullMoveNumber = game.FullMoveNumber,
            HalfMoveClock = game.HalfMoveClock,
            SideToMove = game.SideToMove
        };

        var gameInfoModel = new GameInfoModel()
        {
            Id = gameId,
            WhiteSideUserId = game.WhiteSideUserId,
            BlackSideUserId = game.BlackSideUserId,
            Game = new Game(gameStateModel)
        };

        return gameInfoModel;
    }

    public async Task CreateGame(GameInfoModel gameInfo)
    {
        var game = new GameModel()
        {
            Id = gameInfo.Id,
            WhiteSideUserId = gameInfo.WhiteSideUserId,
            BlackSideUserId = gameInfo.BlackSideUserId,
            State = gameInfo.Game.State,
            Fen = gameInfo.Game.Fen,
            CastlingRightBlackKingSide = gameInfo.Game.CastlingRightBlackKingSide,
            CastlingRightBlackQueenSide = gameInfo.Game.CastlingRightBlackQueenSide,
            CastlingRightWhiteKingSide = gameInfo.Game.CastlingRightWhiteKingSide,
            CastlingRightWhiteQueenSide = gameInfo.Game.CastlingRightWhiteQueenSide,
            FullMoveNumber = gameInfo.Game.FullMoveNumber,
            HalfMoveClock = gameInfo.Game.HalfMoveClock,
            SideToMove = gameInfo.Game.SideToMove,
            EnpassantSquareRank = null,
            EnpassantSquareFile = null,
        };

        _dbContext.Game.Add(game);
        await _dbContext.SaveChangesAsync();
    }

}


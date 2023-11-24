
using SolveChess.Logic.Attributes;
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.DAL;
using SolveChess.Logic.DTO;
using SolveChess.Logic.Interfaces;
using SolveChess.Logic.ResultObjects;

namespace SolveChess.Logic.Service;

public class ChessService : IChessService
{

    private readonly IGameDal _gameDal;

    public ChessService(IGameDal gameDal)
    {
        _gameDal = gameDal;
    }

    public bool UserHasAccessToGame(string gameId, string userId)
    {
        throw new NotImplementedException();
    }

    public MoveResult PlayMoveOnGame(string gameId, string userId, Square from, Square to, PieceType? promotion)
    {
        var gameDto = _gameDal.GetGame(gameId);
        if (gameDto.BlackPlayerId != userId && gameDto.WhitePlayerId != userId)
            return new MoveResult(StatusCode.FAILURE, "User has no access to this game!");

        if ((gameDto.SideToMove != Side.BLACK && gameDto.BlackPlayerId == userId) || (gameDto.SideToMove != Side.WHITE && gameDto.WhitePlayerId == userId))
            return new MoveResult(StatusCode.FAILURE, "It's not the users turn!");

        var game = new Game(gameDto);
        MoveResult moveResult = game.PlayMove(from, to, promotion);
        if (!moveResult.Succeeded)
            return moveResult;

        if (moveResult.MoveDto == null)
            return new MoveResult(StatusCode.FAILURE, "There was an unexpected problem when playing your move!");

        try
        {
            var updatedGameDto = new GameDto()
            {
                Id = gameDto.Id,
                WhitePlayerId = gameDto.WhitePlayerId,
                BlackPlayerId = gameDto.BlackPlayerId,
                State = game.State,
                Fen = game.Fen,
                SideToMove = game.SideToMove,
                FullMoveNumber = game.FullMoveNumber,
                HalfMoveClock = game.HalfMoveClock,
                CastlingRightBlackKingSide = game.CastlingRightBlackKingSide,
                CastlingRightBlackQueenSide = game.CastlingRightBlackQueenSide,
                CastlingRightWhiteKingSide = game.CastlingRightWhiteKingSide,
                CastlingRightWhiteQueenSide = game.CastlingRightWhiteQueenSide,
                EnpassantSquare = game.EnpassantSquare,
            };

            _gameDal.UpdateGame(updatedGameDto);
            _gameDal.AddMove(gameId, moveResult.MoveDto);

            return moveResult;
        }
        catch(Exception exception)
        {
            return new MoveResult(StatusCode.EXCEPTION, exception);
        }
        
    }

    public IEnumerable<MoveDto> GetMoves(string gameId)
    {
        throw new NotImplementedException();
    }

    public void CreateGame()
    {
        throw new NotImplementedException();
    }

}


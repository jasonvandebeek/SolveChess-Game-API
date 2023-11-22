
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.DAL;
using SolveChess.Logic.DTO;

namespace SolveChess.Logic.Service;

public class ChessService
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

    public void PlayMoveOnGame(string gameId, string userId, Square from, Square to, PieceType? promotion)
    {
        var gameDto = _gameDal.GetGame(gameId);
        if ((gameDto.SideToMove != Side.BLACK && gameDto.BlackPlayerId == userId) || (gameDto.SideToMove != Side.WHITE && gameDto.WhitePlayerId == userId))
            return;

        var game = new Game(gameDto);
        MoveDto? moveDto = game.PlayMove(from, to, promotion);
        if (moveDto == null)
            return;

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
        _gameDal.AddMove(gameId, moveDto);
    }

    public IEnumerable<MoveDto> GetMoves(string gameId)
    {
        throw new NotImplementedException();
    }

}


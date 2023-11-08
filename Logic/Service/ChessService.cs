
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Pieces;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.DAL;
using SolveChess.Logic.DTO;

namespace SolveChess.Logic.Service;

public class ChessService
{

    private readonly IGameDAL _gameDAL;

    public ChessService(IGameDAL gameDAL)
    {
        _gameDAL = gameDAL;
    }

    public bool UserHasAccessToGame(string gameId, string userId)
    {
        throw new NotImplementedException();
    }

    public void PlayMoveOnGame(string gameId, string userId, Square from, Square to, PieceType? promotion)
    {
        var gameDTO = _gameDAL.GetGame(gameId);
        if ((gameDTO.SideToMove != Side.BLACK && gameDTO.BlackPlayerId == userId) || (gameDTO.SideToMove != Side.WHITE && gameDTO.WhitePlayerId == userId))
            return;

        var game = new Game(gameDTO);
        MoveDTO moveDTO = game.PlayMove(from, to, promotion);

        var updatedGameDTO = new GameDTO()
        {
            Id = gameDTO.Id,
            WhitePlayerId = gameDTO.WhitePlayerId,
            BlackPlayerId = gameDTO.BlackPlayerId,
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

        _gameDAL.UpdateGame(updatedGameDTO);
        _gameDAL.AddMove(gameId, moveDTO);
    }

    public IEnumerable<MoveDTO> GetMoves(string gameId)
    {
        throw new NotImplementedException();
    }

}


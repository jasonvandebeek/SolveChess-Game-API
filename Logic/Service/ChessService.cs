
using SolveChess.Logic.Attributes;
using SolveChess.Logic.Chess;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Interfaces;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.DAL;
using SolveChess.Logic.Models;
using SolveChess.Logic.Interfaces;
namespace SolveChess.Logic.Service;

public class ChessService : IChessService
{

    private readonly IClientCommunicationService _clientCommunicationService;
    private readonly IGameDal _gameDal;

    public ChessService(IGameDal gameDal, IClientCommunicationService clientCommunicationService)
    {
        _clientCommunicationService = clientCommunicationService;
        _gameDal = gameDal;
    }

    public async Task<bool> UserHasAccessToGame(string gameId, string userId)
    {
        GameInfoModel gameInfoModel = await GetGameWithId(gameId);
        return UserHasAccessToGame(gameInfoModel, userId);
    }

    public async Task<Move?> PlayMoveOnGame(string gameId, string userId, ISquare from, ISquare to, string? promotion)
    { 
        GameInfoModel gameInfoModel = await GetGameWithId(gameId);
        if (!UserHasAccessToGame(gameInfoModel, userId) || !IsUserToMove(gameInfoModel, userId))
            return null;

        PieceType? promotionType = promotion != null ? (PieceType)Enum.Parse(typeof(PieceType), promotion) : null;
        Move? move = gameInfoModel.Game.PlayMove((Square)from, (Square)to, promotionType);
        if (move == null)
            return move;

        await _gameDal.UpdateGame(gameId, gameInfoModel.Game, move);

        await _clientCommunicationService.SendMoveToGame(gameId, move);

        return move;
    }

    public async Task<IEnumerable<Move>> GetPlayedMovesForGame(string gameId)
    {
        throw new NotImplementedException();
    }

    public async Task CreateNewGame(string playerOneUserId, string playerTwoUserId, string? WhiteSideUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<GameInfoModel> GetGameWithId(string gameId)
    {
        return await _gameDal.GetGameWithId(gameId);
    }


    private static bool UserHasAccessToGame(GameInfoModel gameInfoModel, string userId)
    {
        return gameInfoModel.BlackPlayerId != userId || gameInfoModel.WhitePlayerId != userId;
    }

    private static bool IsUserToMove(GameInfoModel gameInfoModel, string userId)
    {
        return (gameInfoModel.Game.SideToMove != Side.BLACK && gameInfoModel.BlackPlayerId == userId) || (gameInfoModel.Game.SideToMove != Side.WHITE && gameInfoModel.WhitePlayerId == userId);
    }

}


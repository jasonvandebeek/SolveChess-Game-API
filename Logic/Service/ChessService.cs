
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
        GameInfoModel? gameInfoModel = await GetGameWithId(gameId);
        if(gameInfoModel == null)
            return false;

        return UserHasAccessToGame(gameInfoModel, userId);
    }

    public async Task<Move?> PlayMoveOnGame(string gameId, string userId, ISquare from, ISquare to, string? promotion)
    { 
        GameInfoModel? gameInfoModel = await GetGameWithId(gameId);
        if (gameInfoModel == null || !UserHasAccessToGame(gameInfoModel, userId) || !IsUserToMove(gameInfoModel, userId))
            return null;

        PieceType? promotionType = promotion != null ? (PieceType)Enum.Parse(typeof(PieceType), promotion) : null;
        var fromSquare = new Square(from.Rank, from.File);
        var toSquare = new Square(to.Rank, to.File);

        Move? move = gameInfoModel.Game.PlayMove(fromSquare, toSquare, promotionType);
        if (move == null)
            return move;

        await _gameDal.UpdateGameAndCreateMove(gameId, gameInfoModel.Game, move);

        await _clientCommunicationService.SendMoveToGame(gameId, move);

        return move;
    }

    public async Task<IEnumerable<Move>?> GetPlayedMovesForGame(string gameId)
    {
        return await _gameDal.GetMovesForGame(gameId);
    }

    public async Task<string?> CreateNewGame(string playerOneUserId, string playerTwoUserId, string? WhiteSideUserId)
    {
        var id = GetNewGameId();
        var game = GetNewGame();

        if (!UserIdsAreValid(playerOneUserId, playerTwoUserId, WhiteSideUserId))
            return null;

        WhiteSideUserId ??= GetWhiteSideUserId(playerOneUserId, playerTwoUserId);
        var blackSideUserId = GetBlackSideUserId(playerOneUserId, playerTwoUserId, WhiteSideUserId);

        var gameInfoModel = new GameInfoModel()
        {
            Id = id,
            WhiteSideUserId = WhiteSideUserId,
            BlackSideUserId = blackSideUserId,
            Game = game,
        };

        await _gameDal.CreateGame(gameInfoModel);

        return id;
    }

    public async Task<GameInfoModel?> GetGameWithId(string gameId)
    {
        return await _gameDal.GetGameWithId(gameId);
    }



    private static string GetNewGameId()
    {
        return Guid.NewGuid().ToString();
    }

    private static Game GetNewGame()
    {
        var gameStateModel = new GameStateModel()
        {
            State = GameState.IN_PROGRESS,
            Fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR",
            CastlingRightBlackKingSide = true,
            CastlingRightBlackQueenSide = true,
            CastlingRightWhiteKingSide = true,
            CastlingRightWhiteQueenSide = true,
            FullMoveNumber = 1,
            HalfMoveClock = 0,
            SideToMove = Side.WHITE
        };

        return new Game(gameStateModel);
    }

    private static bool UserIdsAreValid(string playerOneUserId, string playerTwoUserId, string? whiteSideUserId)
    {
        return playerOneUserId != playerTwoUserId && (whiteSideUserId == playerOneUserId || whiteSideUserId == playerTwoUserId || whiteSideUserId == null);
    }

    private static readonly Random random = new();
    private static string GetWhiteSideUserId(string playerOneUserId, string playerTwoUserId)
    {
        return random.Next(2) == 0 ? playerOneUserId : playerTwoUserId;
    }

    private static string GetBlackSideUserId(string playerOneUserId, string playerTwoUserId, string WhiteSideUserId)
    {
        return WhiteSideUserId == playerOneUserId ? playerTwoUserId : playerOneUserId;
    }

    private static bool UserHasAccessToGame(GameInfoModel gameInfoModel, string userId)
    {
        return gameInfoModel.BlackSideUserId == userId || gameInfoModel.WhiteSideUserId == userId;
    }

    private static bool IsUserToMove(GameInfoModel gameInfoModel, string userId)
    {
        return (gameInfoModel.Game.SideToMove == Side.BLACK && gameInfoModel.BlackSideUserId == userId) || (gameInfoModel.Game.SideToMove == Side.WHITE && gameInfoModel.WhiteSideUserId == userId);
    }

}


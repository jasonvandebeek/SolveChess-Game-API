﻿using SolveChess.Logic.Chess.Interfaces;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.Models;

namespace SolveChess.Logic.Interfaces;

public interface IChessService
{

    Task<Move?> PlayMoveOnGame(string gameId, string userId, ISquare from, ISquare to, string? promotion);
    Task<IEnumerable<Move>?> GetPlayedMovesForGame(string gameId);
    Task<string?> CreateNewGame(string playerOneUserId, string playerTwoUserId, string? WhiteSideUserId);
    Task<GameInfoModel?> GetGameWithId(string gameId);

}

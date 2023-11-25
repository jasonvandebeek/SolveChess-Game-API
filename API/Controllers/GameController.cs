using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolveChess.API.DTO;
using SolveChess.API.Models;
using SolveChess.Logic.Chess.Interfaces;
using SolveChess.Logic.DTO;
using SolveChess.Logic.Interfaces;
using SolveChess.Logic.ResultObjects;

namespace SolveChess.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GameController : Controller
{

    private readonly IChessService _chessService;

    public GameController(IChessService chessService)
    {
        _chessService = chessService;
    }

    [Authorize]
    [HttpPost("{gameId}/move")]
    public async Task<IActionResult> PlayMove(string gameId, [FromBody] MoveDataDto moveDto)
    {
        string? userId = GetUserIdFromCookies();
        if (userId == null)
            return Unauthorized();

        MoveResult moveResult = await _chessService.PlayMoveOnGame(gameId, userId, moveDto.From, moveDto.To, moveDto.Promotion);
        if (!moveResult.Succeeded)
        {
            if (moveResult.Exception != null)
                return StatusCode(500, moveResult.Exception.Message);

            return BadRequest(moveResult.Message);
        }

        return Ok();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateGame([FromBody] GameCreationDto gameCreationDto) //Add creation model
    {
        string? userId = GetUserIdFromCookies();
        if (userId == null)
            return Unauthorized();

        await _chessService.CreateNewGame(gameCreationDto.PlayerOneUserId, gameCreationDto.PlayerTwoUserId, gameCreationDto.WhiteSideUserId);
        return Ok();
    }

    [HttpGet("{gameId}")]
    public async Task<IActionResult> GetGame(string gameId)
    {
        GameInfoModel gameInfo = await _chessService.GetGameWithId(gameId);

        var gameDto = new GameDto()
        {
            Id = gameInfo.Id,
            WhitePlayerId = gameInfo.WhitePlayerId,
            BlackPlayerId = gameInfo.BlackPlayerId,
            State = gameInfo.Game.State.ToString(),
            Fen = gameInfo.Game.Fen,
            SideToMove = gameInfo.Game.SideToMove.ToString(),
            CastlingRightBlackKingSide = gameInfo.Game.CastlingRightBlackKingSide,
            CastlingRightBlackQueenSide = gameInfo.Game.CastlingRightBlackQueenSide,
            CastlingRightWhiteKingSide = gameInfo.Game.CastlingRightWhiteKingSide,
            CastlingRightWhiteQueenSide = gameInfo.Game.CastlingRightWhiteQueenSide,
            EnpassantSquare = (gameInfo.Game.EnpassantSquare as ISquare) as SquareDto
        };

        return Ok(gameDto);
    }


    private string? GetUserIdFromCookies()
    {
        return HttpContext.User.FindFirst("Id")?.Value;
    }

}


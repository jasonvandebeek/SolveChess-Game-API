using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolveChess.API.DTO;
using SolveChess.API.Models;
using SolveChess.Logic.Chess.Interfaces;
using SolveChess.Logic.Models;
using SolveChess.Logic.Interfaces;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.API.Exceptions;

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

        Move? move = await _chessService.PlayMoveOnGame(gameId, userId, moveDto.From, moveDto.To, moveDto.Promotion);
        if(move == null)
            return BadRequest();

        return Ok();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateGame([FromBody] GameCreationDto gameCreationDto)
    {
        string? userId = GetUserIdFromCookies();
        if (userId == null)
            return Unauthorized();

        var id = await _chessService.CreateNewGame(userId, gameCreationDto.OpponentUserId, gameCreationDto.WhiteSideUserId);
        if(id == null)
            return BadRequest();

        return StatusCode(201, id);
    }

    [HttpGet("{gameId}")]
    public async Task<IActionResult> GetGame(string gameId)
    {
        GameInfoModel? gameInfo = await _chessService.GetGameWithId(gameId);
        if(gameInfo == null)
            return NotFound();

        var gameDto = new GameDto()
        {
            Id = gameInfo.Id,
            WhiteSideUserId = gameInfo.WhiteSideUserId,
            BlackSideUserId = gameInfo.BlackSideUserId,
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

    [HttpGet("{gameId}/moves")]
    public async Task<IActionResult> GetMoves(string gameId)
    {
        IEnumerable<Move>? moves = await _chessService.GetPlayedMovesForGame(gameId);
        if (moves == null)
            return NotFound();

        var movesDto = new List<MoveDto>();
        foreach (var move in moves)
        {
            movesDto.Add(new MoveDto()
            {
                Number = move.Number,
                Side = move.Side.ToString(),
                Notation = move.Notation
            });
        }

        return Ok(movesDto);
    }


    private string? GetUserIdFromCookies()
    {
        return HttpContext.User.FindFirst("Id")?.Value;
    }

}


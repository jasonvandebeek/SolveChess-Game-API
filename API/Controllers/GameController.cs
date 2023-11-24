using Google.Protobuf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SolveChess.API.Models;
using SolveChess.API.Websocket;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Interfaces;
using SolveChess.Logic.ResultObjects;
using Square = SolveChess.Logic.Chess.Utilities.Square;

namespace SolveChess.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GameController : Controller
{

    private readonly IHubContext<SignalrHub> _hubContext;
    private readonly IChessService _chessService;

    public GameController(IHubContext<SignalrHub> hubContext, IChessService chessService)
    {
        _hubContext = hubContext;
        _chessService = chessService;
    }

    [Authorize]
    [HttpPost("{gameId}/move")]
    public async Task<IActionResult> PlayMove(string gameId, [FromBody] MoveDataModel move)
    {
        string? userId = HttpContext.User.FindFirst("Id")?.Value;
        if (userId == null)
            return Unauthorized();

        var from = new Square(move.From.Rank, move.From.File);
        var to = new Square(move.To.Rank, move.To.File);
        PieceType? promotionType = move.Promotion != null ? (PieceType) Enum.Parse(typeof(PieceType), move.Promotion) : null;

        MoveResult moveResult = _chessService.PlayMoveOnGame(gameId, userId, from, to, promotionType);
        if (!moveResult.Succeeded)
        {
            if (moveResult.Exception != null)
                return StatusCode(500, moveResult.Exception.Message);

            return BadRequest(moveResult.Message);
        }

        await _hubContext.Clients.Group(gameId).SendAsync("ReceiveMove", moveResult.MoveDto);

        return Ok();
    }

    [Authorize]
    [HttpPost]
    public IActionResult CreateGame()
    {
        string? userId = HttpContext.User.FindFirst("Id")?.Value;
        if (userId == null)
            return Unauthorized();


    }

}


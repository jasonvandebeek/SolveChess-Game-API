using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolveChess.API.Models;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Interfaces;
using Square = SolveChess.Logic.Chess.Utilities.Square;

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
    public IActionResult PlayMove(string gameId, [FromBody] MoveDataModel move)
    {
        string? userId = HttpContext.User.FindFirst("Id")?.Value;
        if (userId == null)
            return Unauthorized();

        var from = new Square(move.From.Rank, move.From.File);
        var to = new Square(move.To.Rank, move.To.File);
        PieceType? promotionType = move.Promotion != null ? (PieceType) Enum.Parse(typeof(PieceType), move.Promotion) : null;

        _chessService.PlayMoveOnGame(gameId, userId, from, to, promotionType);

        return Ok();
    }

}


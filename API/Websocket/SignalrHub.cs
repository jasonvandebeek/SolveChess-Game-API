using Microsoft.AspNetCore.SignalR;
using SolveChess.API.Models;
using SolveChess.Logic.Interfaces;

namespace SolveChess.API.Websocket;

public class SignalrHub : Hub
{

    private readonly IChessService _chessService;

    public SignalrHub(IChessService chessService)
    {
        _chessService = chessService;
    }

    public async Task JoinGame(string gameId)
    {
        //Authenticate user & get userID
        //Check player for gameID

        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
    }

    public async Task SendMove(string gameId, MoveDataModel move)
    {
        await Clients.Group(gameId).SendAsync("RecieveMove", move);
    }

}

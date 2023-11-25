using Microsoft.AspNetCore.SignalR;
using SolveChess.API.Models;

namespace SolveChess.API.Websocket;

public class SignalrHub : Hub
{

    public async Task JoinGame(string gameId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
    }

    public async Task LeaveGame(string gameId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
    }

    public async Task SendMove(string gameId, MoveDataDto move)
    {
        await Clients.Group(gameId).SendAsync("RecieveMove", move);
    }

}

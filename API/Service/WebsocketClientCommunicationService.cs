using Microsoft.AspNetCore.SignalR;
using SolveChess.API.Websocket;
using SolveChess.Logic.Chess.Utilities;
using SolveChess.Logic.Interfaces;

namespace SolveChess.API.Service;

public class WebsocketClientCommunicationService : IClientCommunicationService
{

    private readonly IHubContext<SignalrHub> _hubContext;

    public WebsocketClientCommunicationService(IHubContext<SignalrHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendMoveToGame(string gameId, Move move)
    {
        //TODO: Make move model

        await _hubContext.Clients.Group(gameId).SendAsync("ReceiveMove", move);
    }

}

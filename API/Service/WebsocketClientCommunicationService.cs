using Microsoft.AspNetCore.SignalR;
using SolveChess.API.DTO;
using SolveChess.API.Websocket;
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Interfaces;
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

    public async Task SendMoveToGame(string gameId, Move move, ISquare from, ISquare to, PieceType? promotion)
    {
        var data = new OutgoingMoveDto
        {
            Number = move.Number,
            Side = move.Side.ToString(),
            Notation = move.Notation,
            From = new SquareDto()
            {
                Rank = from.Rank,
                File = from.File
            },
            To = new SquareDto()
            {
                Rank = to.Rank,
                File = to.File
            },
            Promotion = promotion.ToString()
        };

        await _hubContext.Clients.Group(gameId).SendAsync("ReceiveMove", data);
    }

}

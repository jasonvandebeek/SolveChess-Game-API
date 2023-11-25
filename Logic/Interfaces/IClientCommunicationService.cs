
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Interfaces;

public interface IClientCommunicationService
{

    Task SendMoveToGame(string gameId, Move move);

}

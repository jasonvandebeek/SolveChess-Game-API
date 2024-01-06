
using SolveChess.Logic.Chess.Attributes;
using SolveChess.Logic.Chess.Interfaces;
using SolveChess.Logic.Chess.Utilities;

namespace SolveChess.Logic.Interfaces;

public interface IClientCommunicationService
{

    Task SendMoveToGame(string gameId, Move move, ISquare from, ISquare to, PieceType? promotion);

}
